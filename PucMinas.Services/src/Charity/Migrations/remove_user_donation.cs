using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class remove_user_donation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbUser_TbDonation_DonationId",
                table: "TbUser");

            migrationBuilder.DropIndex(
                name: "IX_TbUser_DonationId",
                table: "TbUser");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "TbUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DonationId",
                table: "TbUser",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUser_DonationId",
                table: "TbUser",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUser_TbDonation_DonationId",
                table: "TbUser",
                column: "DonationId",
                principalTable: "TbDonation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
