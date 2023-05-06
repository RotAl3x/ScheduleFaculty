using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleFaculty.Core.Migrations
{
    /// <inheritdoc />
    public partial class Changemaxgroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberOfGroupsOfYears");

            migrationBuilder.AddColumn<int>(
                name: "HowManySemiGroupsAreInAGroup",
                table: "StudyPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSemiGroups",
                table: "StudyPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HowManySemiGroupsAreInAGroup",
                table: "StudyPrograms");

            migrationBuilder.DropColumn(
                name: "NumberOfSemiGroups",
                table: "StudyPrograms");

            migrationBuilder.CreateTable(
                name: "NumberOfGroupsOfYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyProgramYearId = table.Column<Guid>(type: "uuid", nullable: false),
                    HowManySemiGroupsAreInAGroup = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSemiGroups = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfGroupsOfYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberOfGroupsOfYears_StudyPrograms_StudyProgramYearId",
                        column: x => x.StudyProgramYearId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NumberOfGroupsOfYears_StudyProgramYearId",
                table: "NumberOfGroupsOfYears",
                column: "StudyProgramYearId");
        }
    }
}
