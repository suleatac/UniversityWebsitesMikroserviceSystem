using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Personel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    TcNumarasi = table.Column<string>(type: "text", nullable: true),
                    Adi = table.Column<string>(type: "text", nullable: true),
                    Soyadi = table.Column<string>(type: "text", nullable: true),
                    Uyruk = table.Column<string>(type: "text", nullable: true),
                    Cinsiyeti = table.Column<string>(type: "text", nullable: true),
                    Eposta = table.Column<string>(type: "text", nullable: true),
                    TelefonCep = table.Column<string>(type: "text", nullable: true),
                    TelefonDahili = table.Column<string>(type: "text", nullable: true),
                    TelefonDahiliNumara = table.Column<string>(type: "text", nullable: true),
                    Adres = table.Column<string>(type: "text", nullable: true),
                    BabaAdi = table.Column<string>(type: "text", nullable: true),
                    AnaAdi = table.Column<string>(type: "text", nullable: true),
                    DogumYeri = table.Column<string>(type: "text", nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KanGrubu = table.Column<string>(type: "text", nullable: true),
                    SehitGaziYakini = table.Column<string>(type: "text", nullable: true),
                    PersonelTipiId = table.Column<int>(type: "integer", nullable: true),
                    PersonelTipi = table.Column<string>(type: "text", nullable: true),
                    GorevYeriId = table.Column<int>(type: "integer", nullable: true),
                    GorevYeri = table.Column<string>(type: "text", nullable: true),
                    GoreveBaslamaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KurumdanAyrilisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KadroTipiId = table.Column<int>(type: "integer", nullable: true),
                    KadroTipi = table.Column<string>(type: "text", nullable: true),
                    KadroKodu = table.Column<string>(type: "text", nullable: true),
                    IdariGorevler = table.Column<string>(type: "text", nullable: true),
                    IliskiliOlduguPozisyonlar = table.Column<string>(type: "text", nullable: true),
                    EmekliSicilKodu = table.Column<string>(type: "text", nullable: true),
                    UnvanId = table.Column<int>(type: "integer", nullable: true),
                    AsliUnvan = table.Column<string>(type: "text", nullable: true),
                    EkGosterge = table.Column<int>(type: "integer", nullable: true),
                    GorevUnvaniId = table.Column<int>(type: "integer", nullable: true),
                    GorevUnvan = table.Column<string>(type: "text", nullable: true),
                    KadroUnvanId = table.Column<int>(type: "integer", nullable: true),
                    KadroUnvan = table.Column<string>(type: "text", nullable: true),
                    KurumSicilNo = table.Column<string>(type: "text", nullable: true),
                    UstGorevYeriId = table.Column<int>(type: "integer", nullable: true),
                    UstGorevYeriAdi = table.Column<string>(type: "text", nullable: true),
                    UstGorevBirimId = table.Column<int>(type: "integer", nullable: true),
                    UstGorevBirimAdi = table.Column<string>(type: "text", nullable: true),
                    KadroBirimId = table.Column<int>(type: "integer", nullable: true),
                    KadroBirimi = table.Column<string>(type: "text", nullable: true),
                    KadroUstBirimId = table.Column<int>(type: "integer", nullable: true),
                    KadroUstBirim = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    PersonEncryptedId = table.Column<string>(type: "text", nullable: true),
                    BrutUcret = table.Column<decimal>(type: "numeric", nullable: true),
                    SonGuncellemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KisiselEposta = table.Column<string>(type: "text", nullable: true),
                    KisiselTelefon = table.Column<string>(type: "text", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: true),
                    PersonBase64ImageModifiedOn = table.Column<string>(type: "text", nullable: true),
                    PersonBase64Image = table.Column<string>(type: "text", nullable: true),
                    Asili = table.Column<string>(type: "text", nullable: true),
                    CovidBagisik = table.Column<bool>(type: "boolean", nullable: true),
                    SonTestZamani = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    HesDurumu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_Adi_Soyadi",
                table: "Personels",
                columns: new[] { "Adi", "Soyadi" });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_Eposta",
                table: "Personels",
                column: "Eposta");

            migrationBuilder.CreateIndex(
                name: "IX_Personels_SonGuncellemeTarihi",
                table: "Personels",
                column: "SonGuncellemeTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Personels_Username",
                table: "Personels",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personels");
        }
    }
}
