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

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}