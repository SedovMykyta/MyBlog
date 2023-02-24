using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangednamepropertyinArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateLastChangedArticle",
                table: "Articles",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateCreateArticle",
                table: "Articles",
                newName: "DateCreate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Articles",
                newName: "DateLastChangedArticle");

            migrationBuilder.RenameColumn(
                name: "DateCreate",
                table: "Articles",
                newName: "DateCreateArticle");
        }
    }
}
