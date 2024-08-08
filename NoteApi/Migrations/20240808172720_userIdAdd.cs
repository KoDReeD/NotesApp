using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApi.Migrations
{
    /// <inheritdoc />
    public partial class userIdAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WhoCreatedId",
                table: "Tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WhoCreatedId",
                table: "NoteTags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_WhoCreatedId",
                table: "Tags",
                column: "WhoCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_WhoCreatedId",
                table: "NoteTags",
                column: "WhoCreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Accounts_WhoCreatedId",
                table: "NoteTags",
                column: "WhoCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Accounts_WhoCreatedId",
                table: "Tags",
                column: "WhoCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Accounts_WhoCreatedId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Accounts_WhoCreatedId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_WhoCreatedId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_NoteTags_WhoCreatedId",
                table: "NoteTags");

            migrationBuilder.DropColumn(
                name: "WhoCreatedId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "WhoCreatedId",
                table: "NoteTags");
        }
    }
}
