using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fewchangesinexistingentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Articles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Desctiprion",
                table: "Articles",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Articles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Articles",
                newName: "Desctiprion");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Articles",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
