using Artsofte.Models;

namespace Artsofte.Data
{
    /// <summary>
    /// This static class for filling DB the testing data
    /// </summary>
    public class DbInitializer
    {
        public static void Initialize(ArtsofteContext context)
        {
            // Look for any employees
            if (context.Employees.Any())
            {
                return; // DB has been seeded
            }

            var departments = new Department[]
            {
                new Department { Name = "IT", Floor = "10" },
                new Department { Name = "HR", Floor = "9" },
                new Department { Name = "Marketing", Floor = "8" },
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();
            
            var programmingLanguages = new ProgrammingLanguage[]
            {
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "Python" },
            };
            context.ProgrammingLanguages.AddRange(programmingLanguages);
            context.SaveChanges();

            var employees = new Employee[]
            {
                new Employee { Name = "John", Surname = "McLean", Age = 30, Gender = "Male", Department = departments[0], ProgrammingLanguage = programmingLanguages[0] },
                new Employee { Name = "Jane", Surname = "Air", Age = 25, Gender = "Female", Department = departments[1], ProgrammingLanguage = programmingLanguages[1] },
                new Employee { Name = "Bob", Surname = "Marly", Age = 40, Gender = "Male", Department = departments[2], ProgrammingLanguage = programmingLanguages[2] },
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            


        }
    }
}
