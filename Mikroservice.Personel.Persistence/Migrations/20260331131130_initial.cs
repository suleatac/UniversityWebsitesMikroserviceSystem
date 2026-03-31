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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personid = table.Column<int>(type: "integer", nullable: false),
                    tcnumarasi = table.Column<string>(type: "text", nullable: false),
                    adi = table.Column<string>(type: "text", nullable: false),
                    soyadi = table.Column<string>(type: "text", nullable: false),
                    uyruk = table.Column<string>(type: "text", nullable: false),
                    cinsiyeti = table.Column<string>(type: "text", nullable: false),
                    eposta = table.Column<string>(type: "text", nullable: true),
                    telefoncep = table.Column<string>(type: "text", nullable: true),
                    telefondahili = table.Column<string>(type: "text", nullable: true),
                    telefondahilinumara = table.Column<string>(type: "text", nullable: true),
                    adres = table.Column<string>(type: "text", nullable: true),
                    babaadi = table.Column<string>(type: "text", nullable: false),
                    anaadi = table.Column<string>(type: "text", nullable: false),
                    dogumyeri = table.Column<string>(type: "text", nullable: false),
                    dogumtarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kangrubu = table.Column<string>(type: "text", nullable: true),
                    sehitgaziyakini = table.Column<string>(type: "text", nullable: true),
                    personeltipiid = table.Column<int>(type: "integer", nullable: true),
                    personeltipi = table.Column<string>(type: "text", nullable: true),
                    gorevyeriid = table.Column<int>(type: "integer", nullable: true),
                    gorevyeri = table.Column<string>(type: "text", nullable: true),
                    gorevebaslamatarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kurumdanayrilistarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kadrotipiid = table.Column<int>(type: "integer", nullable: true),
                    kadrotipi = table.Column<string>(type: "text", nullable: false),
                    kadrokodu = table.Column<string>(type: "text", nullable: true),
                    idarigorevler = table.Column<string>(type: "text", nullable: true),
                    iliskilioldugupozisyonlar = table.Column<string>(type: "text", nullable: true),
                    emeklisicilkodu = table.Column<string>(type: "text", nullable: true),
                    unvanid = table.Column<int>(type: "integer", nullable: true),
                    asliunvan = table.Column<string>(type: "text", nullable: true),
                    ekgosterge = table.Column<int>(type: "integer", nullable: true),
                    gorevunvaniid = table.Column<int>(type: "integer", nullable: true),
                    gorevunvan = table.Column<string>(type: "text", nullable: true),
                    kadrounvanid = table.Column<int>(type: "integer", nullable: true),
                    kadrounvan = table.Column<string>(type: "text", nullable: false),
                    kurumsicilno = table.Column<string>(type: "text", nullable: true),
                    ustgorevyeriid = table.Column<int>(type: "integer", nullable: true),
                    ustgorevyeriadi = table.Column<string>(type: "text", nullable: true),
                    ustgorevbirimid = table.Column<int>(type: "integer", nullable: true),
                    ustgorevbirimadi = table.Column<string>(type: "text", nullable: true),
                    kadrobirimid = table.Column<int>(type: "integer", nullable: true),
                    kadrobirimi = table.Column<string>(type: "text", nullable: true),
                    kadroustbirimid = table.Column<int>(type: "integer", nullable: true),
                    kadroustbirim = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    personencryptedid = table.Column<string>(type: "text", nullable: false),
                    brutucret = table.Column<decimal>(type: "numeric", nullable: true),
                    songuncellemetarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kisiseleposta = table.Column<string>(type: "text", nullable: true),
                    kisiseltelefon = table.Column<string>(type: "text", nullable: true),
                    aktif = table.Column<bool>(type: "boolean", nullable: true),
                    personbase64imagemodifiedon = table.Column<string>(type: "text", nullable: true),
                    personbase64image = table.Column<string>(type: "text", nullable: true),
                    asili = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_adi_soyadi",
                table: "Personels",
                columns: new[] { "adi", "soyadi" });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_eposta",
                table: "Personels",
                column: "eposta");

            migrationBuilder.CreateIndex(
                name: "IX_Personels_songuncellemetarihi",
                table: "Personels",
                column: "songuncellemetarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Personels_username",
                table: "Personels",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personels");
        }
    }
}
