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
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
