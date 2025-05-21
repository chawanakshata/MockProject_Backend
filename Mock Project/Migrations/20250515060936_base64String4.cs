using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class base64String4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TrainingSelfies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TrainingSelfies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
