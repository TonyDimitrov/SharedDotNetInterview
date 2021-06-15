using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class CompanyNationalityNulableToInterview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "Interviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_NationalityId",
                table: "Interviews",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Nationalities_NationalityId",
                table: "Interviews",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Nationalities_NationalityId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_NationalityId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "Interviews");
        }
    }
}
