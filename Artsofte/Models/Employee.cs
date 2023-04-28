using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artsofte.Data;

namespace Artsofte.Models
{
    /// <summary>
    /// Represents an one of the Employee in the organization. A model for parsing data from the DB.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// The unique ID for each employee.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The first Name of the person
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The last Name of the person
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// The age should be between 18 and 65 years old.
        /// </summary>
        [Range(18, 65, ErrorMessage = "The age sould be between 18 y.o and 65 y.o!")]
        public int Age { get; set; }
        /// <summary>
        /// The employee gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// A foreign key from the <see cref="Models.Department"/> table in the DB that assigns a staff member to a specific department.
        /// </summary>
        [ForeignKey("Departments ")]
        public int DepartmentId { get; set; }
        /// <summary>
        /// A specific instance of the <see cref="Models.Department"/> model uses One-To-Many relationships.
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        ///  A foreign key from the <see cref="Models.ProgrammingLanguage"/> table in the database indicating the programming language that an <see cref="Models.Employee"/> uses for work.
        /// </summary>
        [ForeignKey("ProgrammingLanguages ")]
        public int ProgrammingLanguageId { get; set; }
        /// <summary>
        /// A specific instance of the <see cref="Models.ProgrammingLanguage"/> model uses One-To-Many relationships.
        /// </summary>
        public ProgrammingLanguage ProgrammingLanguage { get; set; }

    }
}
