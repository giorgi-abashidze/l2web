using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace l2web.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlineCache",
                table: "OnlineCache");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "OnlineCache",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineUpdate",
                table: "DataUpdates",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8792),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 789, DateTimeKind.Local).AddTicks(1464));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastDataUpdate",
                table: "DataUpdates",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8279),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 788, DateTimeKind.Local).AddTicks(8964));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccountUpdateTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 332, DateTimeKind.Local).AddTicks(4460),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 783, DateTimeKind.Local).AddTicks(1012));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlineCache",
                table: "OnlineCache",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlineCache",
                table: "OnlineCache");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OnlineCache");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineUpdate",
                table: "DataUpdates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 789, DateTimeKind.Local).AddTicks(1464),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastDataUpdate",
                table: "DataUpdates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 788, DateTimeKind.Local).AddTicks(8964),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 337, DateTimeKind.Local).AddTicks(8279));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccountUpdateTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 5, 17, 1, 49, 44, 783, DateTimeKind.Local).AddTicks(1012),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 5, 17, 4, 2, 1, 332, DateTimeKind.Local).AddTicks(4460));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlineCache",
                table: "OnlineCache",
                columns: new[] { "Online", "TDate" });
        }
    }
}
