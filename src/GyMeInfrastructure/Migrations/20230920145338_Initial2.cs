using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Reactions",
                newName: "ImageUel");

            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "ExtendedUsers",
                newName: "ProfilePictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUel",
                table: "Reactions",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "ExtendedUsers",
                newName: "ProfilePicturePath");
        }
    }
}
