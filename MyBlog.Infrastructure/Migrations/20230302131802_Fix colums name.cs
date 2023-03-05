using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixcolumsname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Articles_ArticleId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislike_Articles_ArticleId",
                table: "Dislike");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislike_Users_UserId",
                table: "Dislike");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Articles_ArticleId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Users_UserId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dislike",
                table: "Dislike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "Likes");

            migrationBuilder.RenameTable(
                name: "Dislike",
                newName: "Dislikes");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Like_UserId",
                table: "Likes",
                newName: "IX_Likes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_ArticleId",
                table: "Likes",
                newName: "IX_Likes_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislike_UserId",
                table: "Dislikes",
                newName: "IX_Dislikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislike_ArticleId",
                table: "Dislikes",
                newName: "IX_Dislikes_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ArticleId",
                table: "Comments",
                newName: "IX_Comments_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislikes_Articles_ArticleId",
                table: "Dislikes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislikes_Users_UserId",
                table: "Dislikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislikes_Articles_ArticleId",
                table: "Dislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislikes_Users_UserId",
                table: "Dislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Like");

            migrationBuilder.RenameTable(
                name: "Dislikes",
                newName: "Dislike");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_UserId",
                table: "Like",
                newName: "IX_Like_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_ArticleId",
                table: "Like",
                newName: "IX_Like_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislikes_UserId",
                table: "Dislike",
                newName: "IX_Dislike_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislikes_ArticleId",
                table: "Dislike",
                newName: "IX_Dislike_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ArticleId",
                table: "Comment",
                newName: "IX_Comment_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dislike",
                table: "Dislike",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Articles_ArticleId",
                table: "Comment",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislike_Articles_ArticleId",
                table: "Dislike",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislike_Users_UserId",
                table: "Dislike",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Articles_ArticleId",
                table: "Like",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Users_UserId",
                table: "Like",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
