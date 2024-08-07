using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApi.Migrations
{
    /// <inheritdoc />
    public partial class tokenStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokedIp",
                table: "RefreshToken",
                newName: "ChangeStatusIp");

            migrationBuilder.RenameColumn(
                name: "RevokedDate",
                table: "RefreshToken",
                newName: "ChangeStatusDate");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RefreshToken",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "ChangeStatusIp",
                table: "RefreshToken",
                newName: "RevokedIp");

            migrationBuilder.RenameColumn(
                name: "ChangeStatusDate",
                table: "RefreshToken",
                newName: "RevokedDate");
        }
    }
}
