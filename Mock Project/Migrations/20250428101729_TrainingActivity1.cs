using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class TrainingActivity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "TrainingActivities");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TrainingActivities",
                newName: "Activity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activity",
                table: "TrainingActivities",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "TrainingActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
