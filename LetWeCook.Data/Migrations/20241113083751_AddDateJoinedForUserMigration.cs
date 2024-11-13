using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetWeCook.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDateJoinedForUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_joined",
                table: "application_user",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_joined",
                table: "application_user");
        }
    }
}
