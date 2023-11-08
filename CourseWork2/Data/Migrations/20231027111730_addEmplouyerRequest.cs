using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class addEmplouyerRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployerRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    post = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    education_id = table.Column<int>(type: "int", nullable: false),
                    salary = table.Column<float>(type: "real", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployerRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployerRequests_Educations_education_id",
                        column: x => x.education_id,
                        principalTable: "Educations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerRequests_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequests_education_id",
                table: "EmployerRequests",
                column: "education_id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequests_StatusId",
                table: "EmployerRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequests_UserId",
                table: "EmployerRequests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployerRequests");
        }
    }
}
