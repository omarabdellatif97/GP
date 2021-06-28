using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class IsDescriptionFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionFile",
                table: "CaseFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDescriptionFile",
                table: "CaseFiles");
        }
    }
}
