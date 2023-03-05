using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageonImageBase64inArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Articles",
                newName: "ImageBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Articles",
                newName: "ImageBase64");
        }
    }
}
