using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class base64User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "Users",
                newName: "ProfilePictureBase64");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "UserImages",
                newName: "Base64Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureBase64",
                table: "Users",
                newName: "ProfilePictureUrl");

            migrationBuilder.RenameColumn(
                name: "Base64Image",
                table: "UserImages",
                newName: "ImageUrl");
        }
    }
}
