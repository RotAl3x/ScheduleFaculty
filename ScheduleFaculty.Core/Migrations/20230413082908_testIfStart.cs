using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleFaculty.Core.Migrations
{
    /// <inheritdoc />
    public partial class testIfStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Statuses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HourStudyOfAYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseHourTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyWeeks = table.Column<List<int>>(type: "integer[]", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<int>(type: "integer", nullable: false),
                    EndTime = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourStudyOfAYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourStudyOfAYears_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HourStudyOfAYears_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HourStudyOfAYears_CourseHourTypes_CourseHourTypeId",
                        column: x => x.CourseHourTypeId,
                        principalTable: "CourseHourTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsOfAStudyHour",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SemiGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    HourStudyOfAYearId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsOfAStudyHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsOfAStudyHour_HourStudyOfAYears_HourStudyOfAYearId",
                        column: x => x.HourStudyOfAYearId,
                        principalTable: "HourStudyOfAYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupsOfAStudyHour_StudyYearGroups_SemiGroupId",
                        column: x => x.SemiGroupId,
                        principalTable: "StudyYearGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfAStudyHour_HourStudyOfAYearId",
                table: "GroupsOfAStudyHour",
                column: "HourStudyOfAYearId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsOfAStudyHour_SemiGroupId",
                table: "GroupsOfAStudyHour",
                column: "SemiGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_HourStudyOfAYears_ClassroomId",
                table: "HourStudyOfAYears",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_HourStudyOfAYears_CourseHourTypeId",
                table: "HourStudyOfAYears",
                column: "CourseHourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HourStudyOfAYears_UserId",
                table: "HourStudyOfAYears",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupsOfAStudyHour");

            migrationBuilder.DropTable(
                name: "HourStudyOfAYears");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Statuses");
        }
    }
}
