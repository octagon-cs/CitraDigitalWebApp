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
        public DbSet<Company> Companies { get; set; }
        public DbSet<Pemeriksaan> Pemeriksaans { get; set; }
        public DbSet<ItemPemeriksaan> ItemPemeiksaans { get; set; }
        public DbSet<PengajuanItem> PengajuanItems { get; set; }
        public DbSet<Pengajuan> Pengajuans { get; set; }
        public DbSet<Persetujuan> Persetujuans { get; set; }
        public DbSet<HasilPemeriksaan> HasilPemeriksaans { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
    }
}
