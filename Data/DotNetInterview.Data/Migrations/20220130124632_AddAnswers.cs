using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class AddAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "AnswerId",
                table: "Likes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Likes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuestionId",
                table: "Likes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "Interviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Interviews",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SalaryType",
                table: "Interviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnswerId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(maxLength: 6000, nullable: true),
                    QuestionId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_AnswerId",
                table: "Likes",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_QuestionId",
                table: "Likes",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnswerId",
                table: "Comments",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IsDeleted",
                table: "Answer",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Answer_AnswerId",
                table: "Comments",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Answer_AnswerId",
                table: "Likes",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Questions_QuestionId",
                table: "Likes",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Answer_AnswerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Answer_AnswerId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Questions_QuestionId",
                table: "Likes");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Likes_AnswerId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_QuestionId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AnswerId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "SalaryType",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
