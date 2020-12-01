using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class add_collumn_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TbCharitableInformation",
                newName: "TransparencyDescription");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerDescription",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mission",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteUrl",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vision",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproverData",
                table: "TbCharitableEntity",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "ManagerDescription",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Mission",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "SiteUrl",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Vision",
                table: "TbCharitableInformation");

            migrationBuilder.RenameColumn(
                name: "TransparencyDescription",
                table: "TbCharitableInformation",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproverData",
                table: "TbCharitableEntity",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
