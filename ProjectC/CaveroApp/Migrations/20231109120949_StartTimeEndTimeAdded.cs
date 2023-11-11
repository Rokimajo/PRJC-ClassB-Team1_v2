using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaveroApp.Migrations
{
    /// <inheritdoc />
    public partial class StartTimeEndTimeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Events",
                newName: "start_time");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "end_time",
                table: "Events",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_time",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Events",
                newName: "time");
        }
    }
}
