using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock_Project.Migrations
{
    /// <inheritdoc />
    public partial class TrainingActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TrainingActivities",
                newName: "ActivityName");

            migrationBuilder.AddColumn<int>(
                name: "DayNumber",
                table: "TrainingActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "TrainingActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayNumber",
                table: "TrainingActivities");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "TrainingActivities");

            migrationBuilder.RenameColumn(
                name: "ActivityName",
                table: "TrainingActivities",
                newName: "Title");
        }
    }
}
