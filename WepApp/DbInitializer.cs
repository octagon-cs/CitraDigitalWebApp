using System;
using System.Collections.Generic;
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
                    dataContext.Roles.Add(new Models.Role { Id=1, Name = "Administrator" });
                    dataContext.Roles.Add(new Models.Role { Id = 2, Name = "Manager" });
                    dataContext.Roles.Add(new Models.Role { Id = 3, Name = "Approval1" });
                    dataContext.Roles.Add(new Models.Role { Id = 4, Name = "HSE" });
                    dataContext.Roles.Add(new Models.Role { Id = 5, Name = "Company" });
                    dataContext.Roles.Add(new Models.Role { Id = 6, Name = "Gate" });
                    dataContext.SaveChanges();
                }

                var pemeriksaaans = dataContext.Pemeriksaans.ToList();
                if (pemeriksaaans == null || pemeriksaaans.Count <= 0)
                {
                    dataContext.Pemeriksaans.Add(new Models.Pemeriksaan
                    {
                        Name = "STNK",
                        Items = new List<Models.ItemPemeriksaan> { new Models.ItemPemeriksaan { Kelengkapan = "STNK", Penjelasan = "Surat Tanda Nomor Kendaraan ", JenisPemeriksaan= Models.JenisPemeriksaan.Dokumen } }
                    });
                    dataContext.Pemeriksaans.Add(new Models.Pemeriksaan
                    {
                        Name = "SIM",
                        Items = new List<Models.ItemPemeriksaan> { new Models.ItemPemeriksaan { Kelengkapan = "SIM", Penjelasan = "Surat Izin Mengemudi", JenisPemeriksaan = Models.JenisPemeriksaan.Dokumen } }
                    });
                    dataContext.Pemeriksaans.Add(new Models.Pemeriksaan
                    {
                        Name = "KEURDLLAJR",
                        Items = new List<Models.ItemPemeriksaan> { new Models.ItemPemeriksaan { Kelengkapan = "KEURDLLAJR", Penjelasan = "KEUR ", JenisPemeriksaan = Models.JenisPemeriksaan.Dokumen } }
                    });

                    dataContext.SaveChanges();
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

                    var roleAdmin = dataContext.Roles.FirstOrDefault(x => x.Name == "Administrator");
                    dataContext.Users.Add(model);
                    var userRole = new Models.UserRole { Role = roleAdmin, User = model };
                    dataContext.Entry(userRole.Role).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    model.UserRoles.Add(userRole);
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