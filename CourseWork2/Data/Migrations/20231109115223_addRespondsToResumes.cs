using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class addRespondsToResumes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Accounts_AccountId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_AccountId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Resumes");

            migrationBuilder.CreateTable(
                name: "AccountResume",
                columns: table => new
                {
                    RespondsId = table.Column<int>(type: "int", nullable: false),
                    ResumesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountResume", x => new { x.RespondsId, x.ResumesId });
                    table.ForeignKey(
                        name: "FK_AccountResume_Accounts_RespondsId",
                        column: x => x.RespondsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountResume_Resumes_ResumesId",
                        column: x => x.ResumesId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountResume_ResumesId",
                table: "AccountResume",
                column: "ResumesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountResume");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Resumes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_AccountId",
                table: "Resumes",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Accounts_AccountId",
                table: "Resumes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
