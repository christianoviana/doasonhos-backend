using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class remove_user_donation_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbDonation_TbUser_UserId",
                table: "TbDonation");

            migrationBuilder.DropIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation");

            migrationBuilder.AddColumn<Guid>(
                name: "DonationId",
                table: "TbUser",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbDonation",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "UserLogin",
                table: "TbDonation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TbDonation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUser_DonationId",
                table: "TbUser",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUser_Login",
                table: "TbUser",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUser_TbDonation_DonationId",
                table: "TbUser",
                column: "DonationId",
                principalTable: "TbDonation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbUser_TbDonation_DonationId",
                table: "TbUser");

            migrationBuilder.DropIndex(
                name: "IX_TbUser_DonationId",
                table: "TbUser");

            migrationBuilder.DropIndex(
                name: "IX_TbUser_Login",
                table: "TbUser");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "TbUser");

            migrationBuilder.DropColumn(
                name: "UserLogin",
                table: "TbDonation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TbDonation");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TbCharitableInformation");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbDonation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TbDonation_TbUser_UserId",
                table: "TbDonation",
                column: "UserId",
                principalTable: "TbUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
