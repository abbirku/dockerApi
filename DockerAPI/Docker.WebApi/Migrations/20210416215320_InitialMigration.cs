using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Docker.WebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebCamImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    CaptureTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCamImages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebCamImages");
        }
    }
}
