using Microsoft.EntityFrameworkCore;
using StudentGradeApi.Models;

namespace StudentGradeApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<StudentCgpa> StudentCgpas { get; set; } // The View

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the read-only View
            modelBuilder.Entity<StudentCgpa>()
                .HasNoKey()
                .ToView("student_cgpa_view");

            modelBuilder.Entity<StudentCgpa>()
                    .HasIndex(s => s.CalculatedCgpa)
                    .HasDatabaseName("IX_StudentCgpas_Cgpa_Descending");

            base.OnModelCreating(modelBuilder);
        }
    }
}
