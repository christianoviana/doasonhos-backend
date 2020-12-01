using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class rename_collumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorPF_CPF",
                table: "TbDonorPF");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TbDonorPF",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TbDonorPF");

            migrationBuilder.AddColumn<string>(
                name: "DonorPF_CPF",
                table: "TbDonorPF",
                nullable: true);
        }
    }
}
