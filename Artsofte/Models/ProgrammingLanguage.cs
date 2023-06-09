﻿using System.ComponentModel.DataAnnotations;

namespace Artsofte.Models
{
    public class ProgrammingLanguage
    {
        /// <summary>
        /// The unique Id or each object
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The name of the programming language can be either a short variant (e.g. [JS]) or the full name (e.g. [JavaScript]).
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets the collection of <see cref="Employee"/> who work in the department.
        /// It uses One-to-Many relationships by EF Core
        /// </summary>
        public ICollection<Employee> Employees { get; set; }
    }
}
