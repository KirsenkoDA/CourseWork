using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork2.Data.Migrations
{
    public partial class addRespondToEmployerRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerRequest_Accounts_AccountId",
                table: "EmployerRequest");

            migrationBuilder.DropIndex(
                name: "IX_EmployerRequest_AccountId",
                table: "EmployerRequest");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "EmployerRequest");

            migrationBuilder.CreateTable(
                name: "AccountEmployerRequest",
                columns: table => new
                {
                    EmployerRequestsId = table.Column<int>(type: "int", nullable: false),
                    RespondsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountEmployerRequest", x => new { x.EmployerRequestsId, x.RespondsId });
                    table.ForeignKey(
                        name: "FK_AccountEmployerRequest_Accounts_RespondsId",
                        column: x => x.RespondsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountEmployerRequest_EmployerRequest_EmployerRequestsId",
                        column: x => x.EmployerRequestsId,
                        principalTable: "EmployerRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountEmployerRequest_RespondsId",
                table: "AccountEmployerRequest",
                column: "RespondsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountEmployerRequest");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "EmployerRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequest_AccountId",
                table: "EmployerRequest",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerRequest_Accounts_AccountId",
                table: "EmployerRequest",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
