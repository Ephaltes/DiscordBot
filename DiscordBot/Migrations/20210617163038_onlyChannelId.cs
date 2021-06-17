using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class onlyChannelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadOnlyEntities",
                table: "UploadOnlyEntities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UploadOnlyEntities");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "UploadOnlyEntities");

            migrationBuilder.AlterColumn<ulong>(
                name: "ChannelId",
                table: "UploadOnlyEntities",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadOnlyEntities",
                table: "UploadOnlyEntities",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadOnlyEntities",
                table: "UploadOnlyEntities");

            migrationBuilder.AlterColumn<ulong>(
                name: "ChannelId",
                table: "UploadOnlyEntities",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UploadOnlyEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<ulong>(
                name: "ServerId",
                table: "UploadOnlyEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadOnlyEntities",
                table: "UploadOnlyEntities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReminderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Creator = table.Column<string>(type: "TEXT", nullable: true),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reminder = table.Column<string>(type: "TEXT", nullable: true),
                    ReminderTimesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderEntities", x => x.Id);
                });
        }
    }
}
