using System.ComponentModel.DataAnnotations;

namespace Artsofte.Models
{
    /// <summary>
    /// Represents a department in the organization. A model for parsing data from the DB.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Gets unique identifier of the department.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets the name of the department.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets the floor where the department is located.
        /// </summary>
        public string Floor { get; set; }

    }
}
