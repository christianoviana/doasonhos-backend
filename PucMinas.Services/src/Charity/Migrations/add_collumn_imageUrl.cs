using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class add_collumn_imageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TbItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TbItem");
        }
    }
}
