using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace l2web.Migrations
{
    public partial class adminpanel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineUpdate",
                table: "DataUpdates",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 810, DateTimeKind.Local).AddTicks(2724),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastDataUpdate",
                table: "DataUpdates",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 810, DateTimeKind.Local).AddTicks(2127),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8279));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccountUpdateTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 808, DateTimeKind.Local).AddTicks(2993),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 332, DateTimeKind.Local).AddTicks(4460));

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2021, 5, 29, 1, 13, 53, 798, DateTimeKind.Local).AddTicks(7828)),
                    BackgroundImage = table.Column<string>(nullable: true),
                    IsVideo = table.Column<bool>(nullable: false),
                    IsPinned = table.Column<bool>(nullable: false),
                    VideoLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineUpdate",
                table: "DataUpdates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8792),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 810, DateTimeKind.Local).AddTicks(2724));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastDataUpdate",
                table: "DataUpdates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8279),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 810, DateTimeKind.Local).AddTicks(2127));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccountUpdateTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 332, DateTimeKind.Local).AddTicks(4460),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 28, 1, 13, 53, 808, DateTimeKind.Local).AddTicks(2993));
        }
    }
}
