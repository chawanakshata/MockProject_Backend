using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class base64String5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TeamSelfies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TeamSelfies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
