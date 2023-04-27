namespace Artsofte.Models.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int DepartmentId { get; set; }
        public int ProgrammingLanguageId { get; set; }
    }
}
