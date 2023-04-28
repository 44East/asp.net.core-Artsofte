using Microsoft.EntityFrameworkCore;
using Artsofte.Models;

namespace Artsofte.Data
{
    /// <summary>
    /// Represents a database context for the Artsofte application, allowing access to the <see cref="Employee"/>, <see cref="ProgrammingLanguage"/>, and <see cref="Department"/> tables.
    /// </summary>
    public class ArtsofteContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArtsofteContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ArtsofteContext(DbContextOptions<ArtsofteContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// The DbSet for the <see cref="Employee"/> table.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }
        /// <summary>
        /// The DbSet for the <see cref="ProgrammingLanguage"/> table.
        /// </summary>
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        /// <summary>
        /// The DbSet for the <see cref="Department"/> table.
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>
        /// // Configures the relationships between the tables using fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder to use for configuring the relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This code sets up the One-to-Many relationships between the entities in the database using a Foreign Key constraint.
            //Specifically, it uses the Fluent API to configure the relationships between the Employee entity and the Department and ProgrammingLanguage entities.
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
