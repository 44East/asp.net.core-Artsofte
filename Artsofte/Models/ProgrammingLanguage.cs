using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
