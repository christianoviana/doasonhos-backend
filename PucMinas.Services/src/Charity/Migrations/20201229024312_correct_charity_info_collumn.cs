using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class correct_charity_info_collumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "CharitableInformation_Vision",
                table: "TbCharitableInformation");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "TbCharitableInformation",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Mission",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerDescription",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransparencyDescription",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerDescription",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "TransparencyDescription",
                table: "TbCharitableInformation");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "TbCharitableInformation",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mission",
                table: "TbCharitableInformation",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CharitableInformation_Vision",
                table: "TbCharitableInformation",
                nullable: true);
        }
    }
}
