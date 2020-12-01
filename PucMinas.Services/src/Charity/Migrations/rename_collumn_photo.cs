using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class rename_collumn_photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "TbItem");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "TbItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "TbItem");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "TbItem",
                nullable: true);
        }
    }
}
