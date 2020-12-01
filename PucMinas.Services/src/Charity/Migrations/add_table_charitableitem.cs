using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PucMinas.Services.Charity.Migrations
{
    public partial class add_table_charitableitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerDescription",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "TransparencyDescription",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Photo01",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Photo02",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Photo03",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "Photo04",
                table: "TbCharitableInformation");

            migrationBuilder.RenameColumn(
                name: "Title_2",
                table: "TbCharitableInformation",
                newName: "Title02");

            migrationBuilder.RenameColumn(
                name: "Title_1",
                table: "TbCharitableInformation",
                newName: "Title01");

            migrationBuilder.RenameColumn(
                name: "Title_4",
                table: "TbCharitableInformation",
                newName: "ImageUrl02");

            migrationBuilder.RenameColumn(
                name: "Title_3",
                table: "TbCharitableInformation",
                newName: "ImagePath02");

            migrationBuilder.AlterColumn<string>(
                name: "Vision",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Values",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CharitableInformation_Values",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CharitableInformation_Vision",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath01",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl01",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: "");

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
                        name: "FK_TbCharitableInformationItem_TbCharitableInformation_CharitableInformationId",
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

            migrationBuilder.CreateIndex(
                name: "IX_TbCharitableInformationItem_ItemId",
                table: "TbCharitableInformationItem",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbCharitableInformationItem");

            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "CharitableInformation_Values",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "CharitableInformation_Vision",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "ImagePath01",
                table: "TbCharitableInformation");

            migrationBuilder.DropColumn(
                name: "ImageUrl01",
                table: "TbCharitableInformation");

            migrationBuilder.RenameColumn(
                name: "Title02",
                table: "TbCharitableInformation",
                newName: "Title_2");

            migrationBuilder.RenameColumn(
                name: "Title01",
                table: "TbCharitableInformation",
                newName: "Title_1");

            migrationBuilder.RenameColumn(
                name: "ImageUrl02",
                table: "TbCharitableInformation",
                newName: "Title_4");

            migrationBuilder.RenameColumn(
                name: "ImagePath02",
                table: "TbCharitableInformation",
                newName: "Title_3");

            migrationBuilder.AlterColumn<string>(
                name: "Values",
                table: "TbCharitableInformation",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Vision",
                table: "TbCharitableInformation",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "TbCharitableInformation",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "ManagerDescription",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "TbCharitableInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransparencyDescription",
                table: "TbCharitableInformation",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo01",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo02",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo03",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo04",
                table: "TbCharitableInformation",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
