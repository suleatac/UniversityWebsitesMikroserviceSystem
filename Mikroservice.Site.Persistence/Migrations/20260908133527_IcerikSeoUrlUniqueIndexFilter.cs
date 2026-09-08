using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IcerikSeoUrlUniqueIndexFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_SiteId_SeoUrl",
                table: "Icerik");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_SeoUrl",
                table: "Icerik",
                columns: new[] { "SiteId", "SeoUrl" },
                unique: true,
                filter: "\"IsDeleted\" = FALSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_SiteId_SeoUrl",
                table: "Icerik");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_SeoUrl",
                table: "Icerik",
                columns: new[] { "SiteId", "SeoUrl" },
                unique: true);
        }
    }
}
