using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    internal class DbInitializer
    {
        internal static Task Initialize(DataContext dataContext)
        {
            try
            {
                dataContext.Database.EnsureCreated();
                var roles = dataContext.Roles.ToList();
                if (roles == null || roles.Count() <= 0)
                {
                    dataContext.Roles.Add(new Models.Role { Name = "Administrator" });
                    dataContext.Roles.Add(new Models.Role { Name = "Manager" });
                    dataContext.Roles.Add(new Models.Role { Name = "Approval1" });
                    dataContext.Roles.Add(new Models.Role { Name = "HSE" });
                    dataContext.Roles.Add(new Models.Role { Name = "Company" });
                    dataContext.Roles.Add(new Models.Role { Name = "Gate" });
                    dataContext.SaveChanges();
                }


                var pemeriksaaans = dataContext.ItemPemeriksaans.ToList();
                if (pemeriksaaans == null || pemeriksaaans.Count <= 0)
                {
                    dataContext.ItemPemeriksaans.Add(new Models.ItemPemeriksaan { Kelengkapan = "STNK", Penjelasan = "Surat Tanda Nomor Kendaraan " });
                    dataContext.ItemPemeriksaans.Add(new Models.ItemPemeriksaan { Kelengkapan = "SIM", Penjelasan = "Surat Izin Mengemudi" });
                    dataContext.ItemPemeriksaans.Add(new Models.ItemPemeriksaan { Kelengkapan = "KEURDLLAJR", Penjelasan = "KEUR KENDARAAN" });
                }




                var users = dataContext.Users.AsEnumerable();
                if (users == null || users.Count() <= 0)
                {
                    var model = new Models.User
                    {
                        FirstName = "Admin",
                        LastName = "",
                        Email = "admin@gmail.com",
                        UserName = "Administrator",
                        Status = true,
                        Password = Helpers.MD5Hash.ToMD5Hash("Admin123")
                    };
                    dataContext.Users.Add(model);

                    dataContext.SaveChanges();

                    var roleAdmin = dataContext.Roles.FirstOrDefault(x => x.Name == "Administrator");
                    if (roleAdmin != null)
                        model.UserRoles.Add(new Models.UserRole { UserId = model.Id, RoleId = roleAdmin.Id });
                    dataContext.SaveChanges();
                }

                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }
    }
}