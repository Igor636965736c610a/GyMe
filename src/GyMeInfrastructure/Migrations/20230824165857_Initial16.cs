using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExtendedUsers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExtendedUsers");
        }
    }
}
