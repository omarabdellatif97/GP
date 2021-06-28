using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddScheduledCaseFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledCases");

            migrationBuilder.CreateTable(
                name: "ScheduledCaseFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledCaseFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledCaseFiles_CaseFiles_CaseFileId",
                        column: x => x.CaseFileId,
                        principalTable: "CaseFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledCaseFiles_CaseFileId",
                table: "ScheduledCaseFiles",
                column: "CaseFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledCaseFiles");

            migrationBuilder.CreateTable(
                name: "ScheduledCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledCases_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledCases_CaseId",
                table: "ScheduledCases",
                column: "CaseId");
        }
    }
}
