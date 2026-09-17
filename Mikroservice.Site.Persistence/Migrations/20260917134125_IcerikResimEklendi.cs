using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IcerikResimEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IcerikResim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IcerikId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ResimUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    YuklemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IcerikResim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IcerikResim_Icerik_IcerikId",
                        column: x => x.IcerikId,
                        principalTable: "Icerik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IcerikResim_IcerikId_Sira",
                table: "IcerikResim",
                columns: new[] { "IcerikId", "Sira" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IcerikResim");
        }
    }
}
