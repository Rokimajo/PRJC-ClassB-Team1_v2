using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaveroApp.Migrations
{
    /// <inheritdoc />
    public partial class RecurringDaysMakeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<DateTime>>(
                name: "RecurringDays",
                table: "Users",
                type: "timestamp with time zone[]",
                nullable: true,
                oldClrType: typeof(List<DateTime>),
                oldType: "timestamp with time zone[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<DateTime>>(
                name: "RecurringDays",
                table: "Users",
                type: "timestamp with time zone[]",
                nullable: false,
                oldClrType: typeof(List<DateTime>),
                oldType: "timestamp with time zone[]",
                oldNullable: true);
        }
    }
}
