using System.ComponentModel.DataAnnotations.Schema;

namespace Artsofte.Models
{
    public class ProgrammingLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
