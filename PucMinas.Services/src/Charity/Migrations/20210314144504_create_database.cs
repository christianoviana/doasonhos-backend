using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class create_database : Migration
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
                    ImagePath = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
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
                    ApproverData = table.Column<DateTime>(type: "datetime", nullable: true),
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
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    CPF = table.Column<string>(maxLength: 20, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: false),
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
                name: "TbApproval",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", maxLength: 10, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TbCharitableInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nickname = table.Column<string>(maxLength: 250, nullable: true),
                    About = table.Column<string>(maxLength: 500, nullable: false),
                    Goal = table.Column<string>(maxLength: 500, nullable: false),
                    ManagerDescription = table.Column<string>(maxLength: 500, nullable: false),
                    TransparencyDescription = table.Column<string>(maxLength: 500, nullable: false),
                    Vision = table.Column<string>(maxLength: 500, nullable: false),
                    Mission = table.Column<string>(maxLength: 500, nullable: false),
                    Values = table.Column<string>(maxLength: 500, nullable: false),
                    PicturePath = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    SiteUrl = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ImagePath01 = table.Column<string>(nullable: false),
                    ImageUrl01 = table.Column<string>(nullable: false),
                    Title01 = table.Column<string>(nullable: false),
                    ImagePath02 = table.Column<string>(nullable: false),
                    ImageUrl02 = table.Column<string>(nullable: false),
                    Title02 = table.Column<string>(nullable: false),
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
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Total = table.Column<double>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    Canceled = table.Column<bool>(nullable: false),
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
                name: "TbCharitableInformationItem",
                columns: table => new
                {
                    CharitableInformationId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCharitableInformationItem", x => new { x.CharitableInformationId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_TbCharitableInformationItem_TbCharitableInformation_Charitab~",
                        column: x => x.CharitableInformationId,
                        principalTable: "TbCharitableInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCharitableInformationItem_TbItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbDonationItem",
                columns: table => new
                {
                    DonationId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
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
                name: "IX_TbApproval_CharitableEntityId",
                table: "TbApproval",
                column: "CharitableEntityId");

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
                name: "IX_TbCharitableInformationItem_ItemId",
                table: "TbCharitableInformationItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_CharitableEntityId",
                table: "TbDonation",
                column: "CharitableEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDonation_UserId",
                table: "TbDonation",
                column: "UserId");

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
                name: "IX_TbUser_Login",
                table: "TbUser",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserRoles_UserId",
                table: "TbUserRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbApproval");

            migrationBuilder.DropTable(
                name: "TbCharitableInformationItem");

            migrationBuilder.DropTable(
                name: "TbDonationItem");

            migrationBuilder.DropTable(
                name: "TbDonorPF");

            migrationBuilder.DropTable(
                name: "TbDonorPJ");

            migrationBuilder.DropTable(
                name: "TbUserRoles");

            migrationBuilder.DropTable(
                name: "TbCharitableInformation");

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
