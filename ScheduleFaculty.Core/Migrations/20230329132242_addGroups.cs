using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleFaculty.Core.Migrations
{
    /// <inheritdoc />
    public partial class addGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudyYearGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SemiGroup = table.Column<int>(type: "integer", nullable: false),
                    Group = table.Column<int>(type: "integer", nullable: false),
                    StudyProgramYearId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyYearGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyYearGroups_StudyPrograms_StudyProgramYearId",
                        column: x => x.StudyProgramYearId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyYearGroups_StudyProgramYearId",
                table: "StudyYearGroups",
                column: "StudyProgramYearId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyYearGroups");
        }
    }
}
