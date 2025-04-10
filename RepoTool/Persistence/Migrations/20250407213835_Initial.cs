using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepoTool.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Changelogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Changes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changelogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InferenceCache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PromptHash = table.Column<string>(type: "TEXT", nullable: false),
                    InferenceProvider = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputType = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseContent = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patterns = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParsedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilePath = table.Column<string>(type: "TEXT", nullable: true),
                    FileHash = table.Column<string>(type: "TEXT", nullable: true),
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParsedFile = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParsedFiles_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Changelogs_CreatedAt",
                table: "Changelogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Changelogs_Id",
                table: "Changelogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Changelogs_LastModifiedAt",
                table: "Changelogs",
                column: "LastModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_CreatedAt",
                table: "InferenceCache",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_Id",
                table: "InferenceCache",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_InferenceProvider",
                table: "InferenceCache",
                column: "InferenceProvider");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_LastModifiedAt",
                table: "InferenceCache",
                column: "LastModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_OutputType",
                table: "InferenceCache",
                column: "OutputType");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_PromptHash",
                table: "InferenceCache",
                column: "PromptHash");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceCache_PromptHash_OutputType_InferenceProvider",
                table: "InferenceCache",
                columns: new[] { "PromptHash", "OutputType", "InferenceProvider" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedAt",
                table: "Languages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Id",
                table: "Languages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LastModifiedAt",
                table: "Languages",
                column: "LastModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                table: "Languages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_CreatedAt",
                table: "ParsedFiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_FileHash",
                table: "ParsedFiles",
                column: "FileHash");

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_FilePath",
                table: "ParsedFiles",
                column: "FilePath");

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_FilePath_FileHash",
                table: "ParsedFiles",
                columns: new[] { "FilePath", "FileHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_Id",
                table: "ParsedFiles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_LanguageId",
                table: "ParsedFiles",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParsedFiles_LastModifiedAt",
                table: "ParsedFiles",
                column: "LastModifiedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Changelogs");

            migrationBuilder.DropTable(
                name: "InferenceCache");

            migrationBuilder.DropTable(
                name: "ParsedFiles");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
