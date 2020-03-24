using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetInterview.Data.Migrations
{
    public partial class AddCascadeDelete09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Interviews_InterviewId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Interviews_InterviewId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Interviews_InterviewId",
                table: "Comments",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Interviews_InterviewId",
                table: "Likes",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Interviews_InterviewId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Interviews_InterviewId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Interviews_InterviewId",
                table: "Comments",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Interviews_InterviewId",
                table: "Likes",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
