using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Docker.WebApi.Migrations
{
    public partial class AddUserIdInWebCamImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WebCamImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WebCamImages");
        }
    }
}
