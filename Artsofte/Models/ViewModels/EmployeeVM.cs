using System.ComponentModel.DataAnnotations;
namespace Artsofte.Models.ViewModels
{
    /// <summary>
    /// This is a view model for <see cref="Employee"/>. It includes only the data fields required for binding data during creation.
    /// </summary>
    public class EmployeeVM
    {
        /// <summary>
        /// The unique ID for each employee.
        /// </summary>
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
        public int Age { get; set; }
        /// <summary>
        /// The employee gender
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// A foreign key from the <see cref="Models.Department"/> table in the DB that assigns a staff member to a specific department.
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        ///  A foreign key from the <see cref="Models.ProgrammingLanguage"/> table in the database indicating the programming language that an <see cref="Models.Employee"/> uses for work.
        /// </summary>
        public int ProgrammingLanguageId { get; set; }
    }
}
