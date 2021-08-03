using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
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
        public DbSet<TruckIncomming> TruckIncommings { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) {
        }

    }


    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, string fieldName) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
                v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("longtext");
            propertyBuilder.HasField(fieldName);

            return propertyBuilder;
        }
    }
}
