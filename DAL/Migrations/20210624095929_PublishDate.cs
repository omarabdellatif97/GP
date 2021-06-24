using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PublishDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Cases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 24, 11, 59, 29, 535, DateTimeKind.Local).AddTicks(1580));

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "CaseFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 24, 11, 59, 29, 538, DateTimeKind.Local).AddTicks(3189));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "CaseFiles");
        }
    }
}
