
using Microsoft.EntityFrameworkCore;
using Artsofte.Models;

namespace Artsofte.Data
{
    public class ArtsofteContext : DbContext
    {
        public ArtsofteContext(DbContextOptions<ArtsofteContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Enrollment>()
                        .HasOne(e => e.Employee)
                        .WithMany(e => e.Enrollments)
                        .HasForeignKey(e => e.EmployeeId)
                        .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<ProgrammingLanguage>().ToTable("ProgrammingLanguage");
            modelBuilder.Entity<Employee>().ToTable("Employee");
        }

    }
}
