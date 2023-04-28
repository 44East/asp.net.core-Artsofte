namespace Artsofte.Models.SortingStates
{
    /// <summary>
    /// The sorting states for the <see cref="Pages.Departments.IndexModel"/> page of <see cref="Department"/> model could be defined as follows:
    /// Sort by name (ascending or descending)
    /// Sort by floor (ascending or descending)
    /// </summary>
    public enum DepartmentSortState
    {
        NameAsc,
        NameDesc, 
        FloorAsc,
        FloorDesc
    }
}
