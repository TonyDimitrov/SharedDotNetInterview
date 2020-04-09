using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class propertyNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeldOnInterviewLocation",
                table: "Interviews");

            migrationBuilder.AddColumn<string>(
                name: "BasedPositionLocation",
                table: "Interviews",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasedPositionLocation",
                table: "Interviews");

            migrationBuilder.AddColumn<string>(
                name: "HeldOnInterviewLocation",
                table: "Interviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
