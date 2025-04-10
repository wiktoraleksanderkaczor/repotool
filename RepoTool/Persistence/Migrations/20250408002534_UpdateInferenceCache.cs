using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepoTool.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInferenceCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InferenceModel",
                table: "InferenceCache",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InferenceModel",
                table: "InferenceCache");
        }
    }
}
