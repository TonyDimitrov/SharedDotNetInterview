using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class nalionalityRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NationalityId",
                table: "AspNetUsers",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Nationalities_NationalityId",
                table: "AspNetUsers",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Nationalities_NationalityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NationalityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "AspNetUsers");
        }
    }
}
