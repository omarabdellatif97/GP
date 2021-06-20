using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Cases_CaseId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_CaseId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "ApplicationCase",
                columns: table => new
                {
                    ApplicationsId = table.Column<int>(type: "int", nullable: false),
                    CasesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationCase", x => new { x.ApplicationsId, x.CasesId });
                    table.ForeignKey(
                        name: "FK_ApplicationCase_Applications_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationCase_Cases_CasesId",
                        column: x => x.CasesId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationCase_CasesId",
                table: "ApplicationCase",
                column: "CasesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationCase");

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CaseId",
                table: "Applications",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Cases_CaseId",
                table: "Applications",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
