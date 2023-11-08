using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class deleteAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequests_Accounts_AccountId",
                table: "EmployerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Accounts_AccountId",
                table: "Resumes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_AccountId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_EmployerRequests_AccountId",
                table: "EmployerRequests");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "EmployerRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Resumes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "EmployerRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_AccountId",
                table: "Resumes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequests_AccountId",
                table: "EmployerRequests",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequests_Accounts_AccountId",
                table: "EmployerRequests",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Accounts_AccountId",
                table: "Resumes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
