using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaveroApp.Migrations
{
    /// <inheritdoc />
    public partial class AltInfoRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alt_info",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "alt_info",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
