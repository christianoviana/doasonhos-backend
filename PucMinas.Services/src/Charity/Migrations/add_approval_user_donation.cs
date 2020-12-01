using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class add_approval_user_donation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation");

            migrationBuilder.DropColumn(
                name: "UserLogin",
                table: "TbDonation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TbDonation");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbDonation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "TbDonation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TbApproval",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(maxLength: 10, nullable: false),
                    Message = table.Column<string>(maxLength: 250, nullable: false),
                    Detail = table.Column<string>(maxLength: 250, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CharitableEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbApproval_TbCharitableEntity_CharitableEntityId",
                        column: x => x.CharitableEntityId,
                        principalTable: "TbCharitableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation",
                column: "CharitableEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbApproval_CharitableEntityId",
                table: "TbApproval",
                column: "CharitableEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbDonation_TbUser_UserId",
                table: "TbDonation",
                column: "UserId",
                principalTable: "TbUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbDonation_TbUser_UserId",
                table: "TbDonation");

            migrationBuilder.DropTable(
                name: "TbApproval");

            migrationBuilder.DropIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation");

            migrationBuilder.DropIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "TbDonation");

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

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation",
                column: "CharitableEntityId",
                unique: true);
        }
    }
}
