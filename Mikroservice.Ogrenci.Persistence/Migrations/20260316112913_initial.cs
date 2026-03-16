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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ogrenciprogramid = table.Column<int>(type: "integer", nullable: false),
                    tcnumarasi = table.Column<string>(type: "text", nullable: true),
                    uyruk = table.Column<string>(type: "text", nullable: true),
                    adi = table.Column<string>(type: "text", nullable: true),
                    soyadi = table.Column<string>(type: "text", nullable: true),
                    cinsiyeti = table.Column<string>(type: "text", nullable: true),
                    eposta = table.Column<string>(type: "text", nullable: true),
                    telefoncep = table.Column<string>(type: "text", nullable: true),
                    adres = table.Column<string>(type: "text", nullable: true),
                    babaadi = table.Column<string>(type: "text", nullable: true),
                    anaadi = table.Column<string>(type: "text", nullable: true),
                    dogumyeri = table.Column<string>(type: "text", nullable: true),
                    dogumtarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kangrubu = table.Column<string>(type: "text", nullable: true),
                    sehitgaziyakini = table.Column<string>(type: "text", nullable: true),
                    resimkodu = table.Column<string>(type: "text", nullable: true),
                    fakulteid = table.Column<int>(type: "integer", nullable: true),
                    fakulte = table.Column<string>(type: "text", nullable: true),
                    bolum = table.Column<string>(type: "text", nullable: true),
                    fakulteingilizceadi = table.Column<string>(type: "text", nullable: true),
                    bolumingilizceadi = table.Column<string>(type: "text", nullable: true),
                    sinif = table.Column<int>(type: "integer", nullable: true),
                    studentno = table.Column<string>(type: "text", nullable: true),
                    akademikprogram = table.Column<string>(type: "text", nullable: true),
                    programtipi = table.Column<string>(type: "text", nullable: true),
                    scholarship = table.Column<string>(type: "text", nullable: true),
                    durum = table.Column<string>(type: "text", nullable: true),
                    ogretimtipi = table.Column<string>(type: "text", nullable: true),
                    programturu = table.Column<string>(type: "text", nullable: true),
                    transcriptnotortalamasi = table.Column<decimal>(type: "numeric", nullable: true),
                    kayittarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    bolumid = table.Column<int>(type: "integer", nullable: true),
                    yoksisogrenciid = table.Column<int>(type: "integer", nullable: true),
                    yoksisid = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    songuncellemetarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kisiseleposta = table.Column<string>(type: "text", nullable: true),
                    kisiseltelefon = table.Column<string>(type: "text", nullable: true),
                    personbase64image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrencis", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ogrencis");
        }
    }
}
