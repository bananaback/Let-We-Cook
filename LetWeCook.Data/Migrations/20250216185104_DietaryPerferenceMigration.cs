using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetWeCook.Data.Migrations
{
    /// <inheritdoc />
    public partial class DietaryPerferenceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "dietary_preference",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "dietary_preference",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "dietary_preference");

            migrationBuilder.DropColumn(
                name: "description",
                table: "dietary_preference");
        }
    }
}
