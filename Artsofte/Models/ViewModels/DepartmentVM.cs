namespace Artsofte.Models.ViewModels
{
    /// <summary>
    /// This is a view model for <see cref="Department"/>. It includes only the data fields required for binding data during creation.
    /// </summary>
    public class DepartmentVM
    {
        /// <summary>
        /// Gets unique identifier of the department.
        /// </summary>
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
