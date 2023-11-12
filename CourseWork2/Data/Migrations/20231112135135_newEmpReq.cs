using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class newEmpReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequests_Accounts_AccountId",
                table: "EmployerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequests_AspNetUsers_UserId",
                table: "EmployerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequests_Educations_education_id",
                table: "EmployerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequests_Statuses_StatusId",
                table: "EmployerRequests");

            migrationBuilder.DropTable(
                name: "EmployerRequestNew");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployerRequests",
                table: "EmployerRequests");

            migrationBuilder.RenameTable(
                name: "EmployerRequests",
                newName: "EmployerRequest");

            migrationBuilder.RenameColumn(
                name: "education_id",
                table: "EmployerRequest",
                newName: "EducationId");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "EmployerRequest",
                newName: "status_id");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequests_UserId",
                table: "EmployerRequest",
                newName: "IX_EmployerRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequests_StatusId",
                table: "EmployerRequest",
                newName: "IX_EmployerRequest_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequests_education_id",
                table: "EmployerRequest",
                newName: "IX_EmployerRequest_EducationId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequests_AccountId",
                table: "EmployerRequest",
                newName: "IX_EmployerRequest_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployerRequest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployerRequest",
                table: "EmployerRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequest_Accounts_AccountId",
                table: "EmployerRequest",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequest_AspNetUsers_UserId",
                table: "EmployerRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequest_Educations_EducationId",
                table: "EmployerRequest",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequest_Statuses_status_id",
                table: "EmployerRequest",
                column: "status_id",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequest_Accounts_AccountId",
                table: "EmployerRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequest_AspNetUsers_UserId",
                table: "EmployerRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequest_Educations_EducationId",
                table: "EmployerRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequest_Statuses_status_id",
                table: "EmployerRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployerRequest",
                table: "EmployerRequest");

            migrationBuilder.RenameTable(
                name: "EmployerRequest",
                newName: "EmployerRequests");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "EmployerRequests",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "EducationId",
                table: "EmployerRequests",
                newName: "education_id");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequest_UserId",
                table: "EmployerRequests",
                newName: "IX_EmployerRequests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequest_status_id",
                table: "EmployerRequests",
                newName: "IX_EmployerRequests_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequest_EducationId",
                table: "EmployerRequests",
                newName: "IX_EmployerRequests_education_id");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerRequest_AccountId",
                table: "EmployerRequests",
                newName: "IX_EmployerRequests_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployerRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployerRequests",
                table: "EmployerRequests",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmployerRequestNew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EducationId = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    salary = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerRequestNew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployerRequestNew_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerRequestNew_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerRequestNew_Statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequestNew_EducationId",
                table: "EmployerRequestNew",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequestNew_status_id",
                table: "EmployerRequestNew",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequestNew_UserId",
                table: "EmployerRequestNew",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequests_Accounts_AccountId",
                table: "EmployerRequests",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequests_AspNetUsers_UserId",
                table: "EmployerRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequests_Educations_education_id",
                table: "EmployerRequests",
                column: "education_id",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequests_Statuses_StatusId",
                table: "EmployerRequests",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
