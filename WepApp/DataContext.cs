using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp
{
    public class DataContext : DbContext
    {
        public DbSet<KIM> Kims { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<ItemPemeriksaan> ItemPemeriksaans { get; set; }
        public DbSet<PengajuanItem> PengajuanItem { get; set; }
        public DbSet<Pengajuan> Pengajuans { get; set; }
        public DbSet<Persetujuan> Persetujuans { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
    }
}
