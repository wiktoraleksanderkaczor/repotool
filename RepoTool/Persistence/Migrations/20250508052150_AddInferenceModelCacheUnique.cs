using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepoTool.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInferenceModelCacheUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InferenceCache_PromptHash_OutputType_InferenceProvider",
                table: "InferenceCache");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_PromptHash_OutputType_InferenceProvider_InferenceModel",
                table: "InferenceCache",
                columns: new[] { "PromptHash", "OutputType", "InferenceProvider", "InferenceModel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InferenceCache_PromptHash_OutputType_InferenceProvider_InferenceModel",
                table: "InferenceCache");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_PromptHash_OutputType_InferenceProvider",
                table: "InferenceCache",
                columns: new[] { "PromptHash", "OutputType", "InferenceProvider" },
                unique: true);
        }
    }
}
