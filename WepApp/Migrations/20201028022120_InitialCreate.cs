using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WepApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NPWP = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    LogoData = table.Column<byte[]>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemPemeriksaans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Kelengkapan = table.Column<string>(nullable: true),
                    Penjelasan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPemeriksaans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pengajuans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    LetterNumber = table.Column<string>(nullable: true),
                    StatusPengajuan = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengajuans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlateNumber = table.Column<string>(nullable: true),
                    Merk = table.Column<string>(nullable: true),
                    CarCreated = table.Column<int>(nullable: false),
                    TruckType = table.Column<string>(nullable: true),
                    DriverName = table.Column<string>(nullable: true),
                    DriverIDCard = table.Column<string>(nullable: true),
                    DriverLicense = table.Column<string>(nullable: true),
                    DriverPhoto = table.Column<string>(nullable: true),
                    AssdriverName = table.Column<string>(nullable: true),
                    AssdriverIDCard = table.Column<string>(nullable: true),
                    AssdriverLicense = table.Column<string>(nullable: true),
                    AssdriverPhoto = table.Column<string>(nullable: true),
                    KeurDLLAJR = table.Column<string>(nullable: true),
                    VehicleRegistration = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PengajuanItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TruckId = table.Column<int>(nullable: false),
                    AttackStatus = table.Column<int>(nullable: false),
                    PengajuanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PengajuanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PengajuanItems_Pengajuans_PengajuanId",
                        column: x => x.PengajuanId,
                        principalTable: "Pengajuans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HasilPemeriksaans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemPengajuanId = table.Column<int>(nullable: false),
                    ItemPemeriksaanId = table.Column<int>(nullable: false),
                    Hasil = table.Column<bool>(nullable: false),
                    TindakLanjut = table.Column<string>(nullable: true),
                    Keterangan = table.Column<string>(nullable: true),
                    PengajuanItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HasilPemeriksaans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HasilPemeriksaans_PengajuanItems_PengajuanItemId",
                        column: x => x.PengajuanItemId,
                        principalTable: "PengajuanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persetujuans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PengajuanItemId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    StatusPersetujuan = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persetujuans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persetujuans_PengajuanItems_PengajuanItemId",
                        column: x => x.PengajuanItemId,
                        principalTable: "PengajuanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HasilPemeriksaans_PengajuanItemId",
                table: "HasilPemeriksaans",
                column: "PengajuanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PengajuanItems_PengajuanId",
                table: "PengajuanItems",
                column: "PengajuanId");

            migrationBuilder.CreateIndex(
                name: "IX_Persetujuans_PengajuanItemId",
                table: "Persetujuans",
                column: "PengajuanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropTable(
                name: "HasilPemeriksaans");

            migrationBuilder.DropTable(
                name: "ItemPemeriksaans");

            migrationBuilder.DropTable(
                name: "Kims");

            migrationBuilder.DropTable(
                name: "Persetujuans");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "PengajuanItems");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Pengajuans");
        }
    }
}
