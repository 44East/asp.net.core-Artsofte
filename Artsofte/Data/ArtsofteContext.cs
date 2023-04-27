
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ProgrammingLanguage)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.ProgrammingLanguageId);
        }

    }
}
