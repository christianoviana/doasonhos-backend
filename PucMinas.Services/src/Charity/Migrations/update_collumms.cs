using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class update_collumms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharitableInformation_Values",
                table: "TbCharitableInformation",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "TbCharitableInformation",
                newName: "CharitableInformation_Values");
        }
    }
}
