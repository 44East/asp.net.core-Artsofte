using System.ComponentModel.DataAnnotations.Schema;

namespace Artsofte.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        [ForeignKey("Depatments ")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("ProgrammingLanguages ")]
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }

    }
}
