using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApi.Migrations
{
    /// <inheritdoc />
    public partial class renameWhoCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Accounts_WhoCreatedId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Accounts_WhoUpdatedId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Accounts_WhoCreatedId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Accounts_WhoCreatedId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Notes_WhoUpdatedId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WhoUpdatedId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "WhoCreatedId",
                table: "Tags",
                newName: "AccountCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_WhoCreatedId",
                table: "Tags",
                newName: "IX_Tags_AccountCreatedId");

            migrationBuilder.RenameColumn(
                name: "WhoCreatedId",
                table: "NoteTags",
                newName: "AccountCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_NoteTags_WhoCreatedId",
                table: "NoteTags",
                newName: "IX_NoteTags_AccountCreatedId");

            migrationBuilder.RenameColumn(
                name: "WhoCreatedId",
                table: "Notes",
                newName: "AccountCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_WhoCreatedId",
                table: "Notes",
                newName: "IX_Notes_AccountCreatedId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tags",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Accounts_AccountCreatedId",
                table: "Notes",
                column: "AccountCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Accounts_AccountCreatedId",
                table: "NoteTags",
                column: "AccountCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Accounts_AccountCreatedId",
                table: "Tags",
                column: "AccountCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Accounts_AccountCreatedId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Accounts_AccountCreatedId",
                table: "NoteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Accounts_AccountCreatedId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "AccountCreatedId",
                table: "Tags",
                newName: "WhoCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_AccountCreatedId",
                table: "Tags",
                newName: "IX_Tags_WhoCreatedId");

            migrationBuilder.RenameColumn(
                name: "AccountCreatedId",
                table: "NoteTags",
                newName: "WhoCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_NoteTags_AccountCreatedId",
                table: "NoteTags",
                newName: "IX_NoteTags_WhoCreatedId");

            migrationBuilder.RenameColumn(
                name: "AccountCreatedId",
                table: "Notes",
                newName: "WhoCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_AccountCreatedId",
                table: "Notes",
                newName: "IX_Notes_WhoCreatedId");

            migrationBuilder.AddColumn<int>(
                name: "WhoUpdatedId",
                table: "Notes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_WhoUpdatedId",
                table: "Notes",
                column: "WhoUpdatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Accounts_WhoCreatedId",
                table: "Notes",
                column: "WhoCreatedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Accounts_WhoUpdatedId",
                table: "Notes",
                column: "WhoUpdatedId",
                principalTable: "Accounts",
                principalColumn: "Id");

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
    }
}
