using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                newName: "idx_books_author");

            migrationBuilder.CreateIndex(
                name: "idx_books_pagination",
                table: "Books",
                columns: new[] { "Title", "Id" });

            migrationBuilder.CreateIndex(
                name: "idx_books_title",
                table: "Books",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_books_pagination",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "idx_books_title",
                table: "Books");

            migrationBuilder.RenameIndex(
                name: "idx_books_author",
                table: "Books",
                newName: "IX_Books_AuthorId");
        }
    }
}
