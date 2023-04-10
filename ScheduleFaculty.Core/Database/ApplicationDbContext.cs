using System.Collections.Specialized;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Entities;

namespace ScheduleFaculty.Core.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<StudyProgram> StudyPrograms { get; set; }

    public DbSet<StudyYearGroup> StudyYearGroups { get; set; }

    public DbSet<NumberOfGroupsOfYear> NumberOfGroupsOfYears { get; set; }

    public DbSet<Course> Courses { get; set; } //

    public DbSet<HourType> HourTypes { get; set; } //

    public DbSet<CourseHourType> CourseHourTypes { get; set; } //

    public DbSet<Status> Statuses { get; set; }

    public DbSet<HourStudyOfAYear> HourStudyOfAYears { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}