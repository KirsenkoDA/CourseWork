using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class alterResume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Educations_education_id",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Statuses_StatusId",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "education_id",
                table: "Resumes",
                newName: "EducationId");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Resumes",
                newName: "status_id");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_StatusId",
                table: "Resumes",
                newName: "IX_Resumes_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_education_id",
                table: "Resumes",
                newName: "IX_Resumes_EducationId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Resumes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Educations_EducationId",
                table: "Resumes",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Statuses_status_id",
                table: "Resumes",
                column: "status_id",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Educations_EducationId",
                table: "Resumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Statuses_status_id",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "Resumes",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "EducationId",
                table: "Resumes",
                newName: "education_id");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_status_id",
                table: "Resumes",
                newName: "IX_Resumes_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_EducationId",
                table: "Resumes",
                newName: "IX_Resumes_education_id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Resumes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Educations_education_id",
                table: "Resumes",
                column: "education_id",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Statuses_StatusId",
                table: "Resumes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
