using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetWeCook.Data.Migrations
{
    /// <inheritdoc />
    public partial class DietaryPreferenceIconMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "dietary_preference",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "dietary_preference");
        }
    }
}
