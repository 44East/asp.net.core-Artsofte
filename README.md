<h2>Software Description:</h2>   
This software uses <a href="https://learn.microsoft.com/en-gb/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0">ASP.NET Core 6</a>  and <a href="https://learn.microsoft.com/en-gb/aspnet/core/razor-pages/?view=aspnetcore-6.0&tabs=visual-studio">Razor Pages</a>  (for the view) web interface to connect to a local instance of MSSQL Server and work with three entities: "Employees", "Departments", and "Programming Languages". For interacting with the database, <a href="https://learn.microsoft.com/ru-ru/dotnet/framework/data/adonet/">ADO.NET</a>  technology is used. The web interface allows performing all CRUD operations with all entities.

<h2>Compilation:</h2>   
To compile, you need to build the solution before the first run so that all SQL scripts are copied to the application output directory. After that, you need to update all NuGet packages for the application to work correctly, and then you can compile.
