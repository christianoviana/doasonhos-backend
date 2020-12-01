using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ActivationCode = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItem_TbGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TbGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCharitableEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    CNPJ = table.Column<string>(maxLength: 20, nullable: false),
                    RepresentantName = table.Column<string>(maxLength: 150, nullable: false),
                    CellPhone = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Approver = table.Column<string>(maxLength: 150, nullable: true),
                    ApproverData = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(maxLength: 200, nullable: false),
                    State = table.Column<string>(maxLength: 200, nullable: false),
                    Country = table.Column<string>(maxLength: 200, nullable: false),
                    CEP = table.Column<string>(maxLength: 25, nullable: false),
                    AddressName = table.Column<string>(maxLength: 200, nullable: false),
                    District = table.Column<string>(maxLength: 200, nullable: false),
                    Number = table.Column<string>(maxLength: 15, nullable: false),
                    Complement = table.Column<string>(maxLength: 150, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCharitableEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCharitableEntity_TbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TbUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbDonorPF",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CPF = table.Column<string>(maxLength: 20, nullable: false),
                    DonorPF_CPF = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(maxLength: 15, nullable: false),
                    Genre = table.Column<string>(maxLength: 50, nullable: false),
                    City = table.Column<string>(maxLength: 200, nullable: false),
                    State = table.Column<string>(maxLength: 200, nullable: false),
                    Country = table.Column<string>(maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDonorPF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbDonorPF_TbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TbUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbDonorPJ",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 250, nullable: false),
                    ContactName = table.Column<string>(maxLength: 250, nullable: false),
                    CNPJ = table.Column<string>(maxLength: 20, nullable: false),
                    City = table.Column<string>(maxLength: 200, nullable: false),
                    State = table.Column<string>(maxLength: 200, nullable: false),
                    Country = table.Column<string>(maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDonorPJ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbDonorPJ_TbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TbUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TbUserRoles_TbRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TbRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbUserRoles_TbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TbUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCharitableInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nickname = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Picture = table.Column<byte[]>(nullable: true),
                    Photo01 = table.Column<byte[]>(nullable: false),
                    Title_1 = table.Column<string>(nullable: false),
                    Photo02 = table.Column<byte[]>(nullable: false),
                    Title_2 = table.Column<string>(nullable: false),
                    Photo03 = table.Column<byte[]>(nullable: false),
                    Title_3 = table.Column<string>(nullable: false),
                    Photo04 = table.Column<byte[]>(nullable: false),
                    Title_4 = table.Column<string>(nullable: false),
                    CharitableEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCharitableInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCharitableInformation_TbCharitableEntity_CharitableEntityId",
                        column: x => x.CharitableEntityId,
                        principalTable: "TbCharitableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbDonation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CharitableEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDonation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbDonation_TbCharitableEntity_CharitableEntityId",
                        column: x => x.CharitableEntityId,
                        principalTable: "TbCharitableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbDonation_TbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TbUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbDonationItem",
                columns: table => new
                {
                    DonationId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDonationItem", x => new { x.DonationId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_TbDonationItem_TbDonation_DonationId",
                        column: x => x.DonationId,
                        principalTable: "TbDonation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbDonationItem_TbItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCharitableEntity_UserId",
                table: "TbCharitableEntity",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCharitableInformation_CharitableEntityId",
                table: "TbCharitableInformation",
                column: "CharitableEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation",
                column: "CharitableEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDonationItem_ItemId",
                table: "TbDonationItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDonorPF_UserId",
                table: "TbDonorPF",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDonorPJ_UserId",
                table: "TbDonorPJ",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItem_GroupId",
                table: "TbItem",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserRoles_UserId",
                table: "TbUserRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbCharitableInformation");

            migrationBuilder.DropTable(
                name: "TbDonationItem");

            migrationBuilder.DropTable(
                name: "TbDonorPF");

            migrationBuilder.DropTable(
                name: "TbDonorPJ");

            migrationBuilder.DropTable(
                name: "TbUserRoles");

            migrationBuilder.DropTable(
                name: "TbDonation");

            migrationBuilder.DropTable(
                name: "TbItem");

            migrationBuilder.DropTable(
                name: "TbRole");

            migrationBuilder.DropTable(
                name: "TbCharitableEntity");

            migrationBuilder.DropTable(
                name: "TbGroup");

            migrationBuilder.DropTable(
                name: "TbUser");
        }
    }
}
