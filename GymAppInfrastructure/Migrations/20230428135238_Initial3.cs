using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Premiums");

            migrationBuilder.AddColumn<DateTime>(
                name: "ImportancePremium",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Premium",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportancePremium",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Premiums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Importance = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PremiumAccount = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premiums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Premiums_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Premiums_UserId",
                table: "Premiums",
                column: "UserId",
                unique: true);
        }
    }
}
