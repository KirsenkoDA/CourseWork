using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class deleteAccount4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerRequestId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResumeId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerRequestId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "Accounts");
        }
    }
}
