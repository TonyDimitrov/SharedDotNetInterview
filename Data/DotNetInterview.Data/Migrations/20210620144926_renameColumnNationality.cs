using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class renameColumnNationality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "AspNetUsers",
                newName: "UserNationality");

            migrationBuilder.AlterColumn<string>(
                name: "UserNationality",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserNationality",
                table: "AspNetUsers",
                newName: "Nationality");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
