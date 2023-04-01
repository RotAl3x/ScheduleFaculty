using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleFaculty.Core.Migrations
{
    /// <inheritdoc />
    public partial class testallnetestedv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyProgramYearId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfessorUserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_ProfessorUserId",
                        column: x => x.ProfessorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_StudyPrograms_StudyProgramYearId",
                        column: x => x.StudyProgramYearId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HourTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SemiGroupsPerHour = table.Column<int>(type: "integer", nullable: false),
                    NeedAllSemiGroups = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseHourTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    HourTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalHours = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseHourTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseHourTypes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseHourTypes_HourTypes_HourTypeId",
                        column: x => x.HourTypeId,
                        principalTable: "HourTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseHourTypes_CourseId",
                table: "CourseHourTypes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHourTypes_HourTypeId",
                table: "CourseHourTypes",
                column: "HourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProfessorUserId",
                table: "Courses",
                column: "ProfessorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StudyProgramYearId",
                table: "Courses",
                column: "StudyProgramYearId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseHourTypes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "HourTypes");
        }
    }
}
