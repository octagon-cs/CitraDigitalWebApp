using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;                                                                                                                      
using System.Threading;
using System.Threading.Tasks;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy;
using WebApp.Proxy.Domains;
using WebApp.Services;
using WepApp.Models;

namespace WepApp.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private List<KIMState> states;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger,  IServiceProvider serviceProvider)
        {
            _logger = logger;
            //  administrator = UserProxy.GetAdministratorProxy(null, userService);
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            states = new List<KIMState>();
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IUserService userService =
                    scope.ServiceProvider.GetRequiredService<IUserService>();

               var administrator = UserProxy.GetAdministratorProxy(null, userService);
                var kims = administrator.GetAllKIM().Result;

                foreach (var item in kims)
                {
                    ProccessKIM(item, states.Where(x=>x.Id==item.Id).FirstOrDefault());
                }

            }
        }

        private Task ProccessKIM(KIM item, KIMState kimState)
        {
            if (kimState == null)
            {
                kimState = new KIMState() { Id=item.Id};
                states.Add(kimState);
            }

            //umur KIM
            if (!kimState.KIM)
            {
                var ageOfKIM = new Age(DateTime.Now, item.EndDate);
                if (item.EndDate > DateTime.Now && ageOfKIM.Years == 0 && ageOfKIM.Months <= 1)
                {
                    SendMessageOFKIM(item, ageOfKIM, "KIM");
                    kimState.KIM= true;
                }
            }


          


            if (!kimState.Truck)
            {
                if ((DateTime.Now.Year - item.Truck.TahunPembuatan) <= 1)
                {
                    SendMessageOFKIM(item,null, "Kendaraan");
                    kimState.Truck= true;
                }
            }

            //umur Kendaraan
           
            if (!kimState.Driver)
            {
                var ageOfDriver = item.Truck.DriverAge;
                if (ageOfDriver.Years == 49 && ageOfDriver.Months >= 11)
                {
                    SendMessageOFKIM(item, ageOfDriver, "Driver");
                    kimState.Driver= true;
                }
            }

            //age of Driver



            if (!kimState.AssDriver)
            {
                var ageOfAssDriver = item.Truck.AssDriverAge;
                if (ageOfAssDriver.Years == 54 && ageOfAssDriver.Months >= 11)
                {
                    SendMessageOFKIM(item, ageOfAssDriver, "AssDriver");
                    kimState.AssDriver= true;
                }
            }
           
            _logger.LogInformation($"Iterasi  {executionCount++}");
            return Task.CompletedTask;

            //Send email 
        }

        private void SendMessageOFKIM(KIM kim,Age age, string subject)
        {
            using (var service = _serviceProvider.CreateScope())
            {
                var emailService = service.ServiceProvider.GetRequiredService<IEmailService>();
                string template = "";
                switch (subject)
                {
                    case "KIM":
                        template = @" 
                                        <h2>Masa Berlaku Kim Akan Segara Berakhir</h2>
                                        <div class='inputData'>
                                        <label>Nomor KIM </label>
                                        <h4>[nokim]</h4>
                                        </div>

                                        <div class='inputData'>
                                        <label>Tanggal Berakhir Kim</label>
                                        <h4>[tanggal]</h4>
                                        </div>
                                        ";

                        template = template
                         .Replace("[nokim]", kim.KimNumber)
                         .Replace("[tanggal]", kim.EndDate.ToString())
                         ;
                        break;


                    case "Kendaraan":
                        template = @" <h2>Umur Kendaraan Anda Akan Segera 10 Tahun</h2>";
                        break;


                    case "Driver":
                        template = @" 
                                        <h2>Umur Sopir Akan Melewati Batas Maximum !</h2>
                                        <div class='inputData'>
                                        <label>Nama Supir </label>
                                        <h4>[sopir]</h4>
                                        </div>

                                        <div class='inputData'>
                                        <label>Umur Sekarang </label>
                                        <h4>[tanggal]</h4>
                                        </div>
                                        ";

                        template = template
                         .Replace("[sopir]", kim.Truck.DriverName)
                         .Replace("[tanggal]", $"{age.Years} Tahun, {age.Months} Bulan, {age.Days} Hari")
                         ;
                        break;

                    case "AssDriver":
                        template = @" 
                                        <h2>Umur Assisten Driver/Kernet Akan Melewati Batas Maximum ! </h2>
                                        <div class='inputData'>
                                        <label>Nama Asisten Supir/Kernet</label>
                                        <h4>[assopir]</h4>
                                        </div>
                                        <div class='inputData'>
                                        <label>Tanggal Berakhir Kim</label>
                                        <h4>[tanggal]</h4>
                                        </div>
                                        ";

                        template = template
                         .Replace("[assopir]", kim.Truck.AssdriverName)
                         .Replace("[tanggal]", $"{age.Years} Tahun, {age.Months} Bulan, {age.Days} Hari")
                         ;
                        break;
                }
               template = Helpers.EmailHelper.CreateNotification(template);
               emailService.SendEmail(new MailRequest { ToEmail = kim.Truck.Company.Email, Subject = "Masa Berlaku Kim Akan Berakhir !", Body = template });
            }
        }

        public int CalculateAgeCorrect2(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            // For leap years we need this
            if (birthDate > DateTime.Now.AddYears(-age))
                age--;
            // Don't use:
            // if (birthDate.AddYears(age) > now) 
            //     age--;

            return age;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }




    public class KIMState
    {
        public int Id { get; set; }
        public bool KIM{ get; set; }
        public bool Truck{ get; set; }
        public bool Driver { get; set; }
        public bool AssDriver { get; set; }

    }


  
}
