namespace Artsofte.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
