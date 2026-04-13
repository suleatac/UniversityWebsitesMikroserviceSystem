using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservice.Yonetici.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YoneticiDuyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoneticiDuyurular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoneticiTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoneticiTipleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YoneticiDuyurular");

            migrationBuilder.DropTable(
                name: "YoneticiTipleri");
        }
    }
}
