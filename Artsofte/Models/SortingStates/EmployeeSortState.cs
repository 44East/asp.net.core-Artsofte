namespace Artsofte.Models.SortingStates
{
    /// <summary>
    /// The sorting states for the <see cref="Pages.Employees.IndexModel"/> page of <see cref="Employee"/> model could be defined as follows:
    /// Sort by name (ascending or descending)
    /// Sort by surname (ascending or descending)
    /// Sort by age (ascending or descending)
    /// Sort by gender (ascending or descending)
    /// Sort by department (ascending or descending)
    /// Sort by programming language (ascending or descending)
    /// </summary>
    public enum EmployeeSortState
    {
        //Asc - ascending | Desc - descending
        NameAsc,
        NameDesc,
        SurnameAsc,
        SurnameDesc,
        AgeAsc,
        AgeDesc,
        GenderAsc,
        GenderDesc,
        DepartmentAsc,
        DepartmentDesc,
        ProgLangAsc,
        ProgLangDesc
    }
}
