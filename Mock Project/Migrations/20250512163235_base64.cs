using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class base64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureBase64",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Base64Data",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureBase64",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Base64Data",
                table: "UserImages");
        }
    }
}
