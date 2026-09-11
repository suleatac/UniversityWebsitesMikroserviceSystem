using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KisaAciklama",
                table: "ShortcutButtons",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KisaAciklama",
                table: "ShortcutButtons");
        }
    }
}
