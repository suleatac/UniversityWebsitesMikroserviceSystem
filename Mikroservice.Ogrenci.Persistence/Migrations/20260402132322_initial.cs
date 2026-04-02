using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Ogrenci.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ogrencis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    SonGuncellemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KisiselEposta = table.Column<string>(type: "text", nullable: true),
                    KisiselTelefon = table.Column<string>(type: "text", nullable: true),
                    OgrenciProgramId = table.Column<int>(type: "integer", nullable: true),
                    TcNumarasi = table.Column<string>(type: "text", nullable: true),
                    Uyruk = table.Column<string>(type: "text", nullable: true),
                    Adi = table.Column<string>(type: "text", nullable: true),
                    Soyadi = table.Column<string>(type: "text", nullable: true),
                    Cinsiyeti = table.Column<string>(type: "text", nullable: true),
                    Eposta = table.Column<string>(type: "text", nullable: true),
                    TelefonCep = table.Column<string>(type: "text", nullable: true),
                    Adres = table.Column<string>(type: "text", nullable: true),
                    BabaAdi = table.Column<string>(type: "text", nullable: true),
                    AnaAdi = table.Column<string>(type: "text", nullable: true),
                    DogumYeri = table.Column<string>(type: "text", nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    KanGrubu = table.Column<string>(type: "text", nullable: true),
                    SehitGaziYakini = table.Column<string>(type: "text", nullable: true),
                    ResimKodu = table.Column<string>(type: "text", nullable: true),
                    FakulteId = table.Column<int>(type: "integer", nullable: true),
                    Fakulte = table.Column<string>(type: "text", nullable: true),
                    Bolum = table.Column<string>(type: "text", nullable: true),
                    FakulteIngilizceAdi = table.Column<string>(type: "text", nullable: true),
                    BolumIngilizceAdi = table.Column<string>(type: "text", nullable: true),
                    Sinif = table.Column<int>(type: "integer", nullable: true),
                    StudentNo = table.Column<string>(type: "text", nullable: true),
                    PersonBase64Image = table.Column<string>(type: "text", nullable: true),
                    AkademikProgram = table.Column<string>(type: "text", nullable: true),
                    ProgramTipi = table.Column<string>(type: "text", nullable: true),
                    Scholarship = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<string>(type: "text", nullable: true),
                    DurumDetail = table.Column<string>(type: "text", nullable: true),
                    MezuniyetTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IlisikKesmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OgretimTipi = table.Column<string>(type: "text", nullable: true),
                    ProgramTuru = table.Column<string>(type: "text", nullable: true),
                    TranscriptNotOrtalamasi = table.Column<decimal>(type: "numeric", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BolumId = table.Column<int>(type: "integer", nullable: true),
                    YoksisOgrenciId = table.Column<int>(type: "integer", nullable: true),
                    YoksisId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrencis", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_Adi_Soyadi",
                table: "Ogrencis",
                columns: new[] { "Adi", "Soyadi" });

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_Eposta",
                table: "Ogrencis",
                column: "Eposta");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_OgrenciProgramId",
                table: "Ogrencis",
                column: "OgrenciProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_SonGuncellemeTarihi",
                table: "Ogrencis",
                column: "SonGuncellemeTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_Username",
                table: "Ogrencis",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ogrencis");
        }
    }
}
