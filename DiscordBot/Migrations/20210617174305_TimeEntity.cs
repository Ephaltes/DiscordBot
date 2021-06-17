﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class TimeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChannelToPostId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventEntityTimeEntity",
                columns: table => new
                {
                    EventEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeEntitiesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEntityTimeEntity", x => new { x.EventEntitiesId, x.TimeEntitiesId });
                    table.ForeignKey(
                        name: "FK_EventEntityTimeEntity_EventEntities_EventEntitiesId",
                        column: x => x.EventEntitiesId,
                        principalTable: "EventEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEntityTimeEntity_TimeEntities_TimeEntitiesId",
                        column: x => x.TimeEntitiesId,
                        principalTable: "TimeEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEntityTimeEntity_TimeEntitiesId",
                table: "EventEntityTimeEntity",
                column: "TimeEntitiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEntityTimeEntity");

            migrationBuilder.DropTable(
                name: "EventEntities");

            migrationBuilder.DropTable(
                name: "TimeEntities");
        }
    }
}
