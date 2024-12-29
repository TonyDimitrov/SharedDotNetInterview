using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetInterview.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerId",
                table: "Interviews",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_EmployerId",
                table: "Interviews",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_IsDeleted",
                table: "Employers",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Employers_EmployerId",
                table: "Interviews",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Employers_EmployerId",
                table: "Interviews");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_EmployerId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Interviews");
        }
    }
}
