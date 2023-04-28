namespace Artsofte.Models.ViewModels
{
    /// <summary>
    /// This is a view model for <see cref="ProgrammingLanguage"/>. It includes only the data fields required for binding data during creation.
    /// </summary>
    public class ProgrammingLanguageVM
    {
        /// <summary>
        /// The unique Id or each object
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the programming language can be either a short variant (e.g. [JS]) or the full name (e.g. [JavaScript]).
        /// </summary>
        public string Name { get; set; }
    }
}
