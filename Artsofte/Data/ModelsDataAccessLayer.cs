using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data;
using Artsofte.Models;
using System.Text;
using Artsofte.Models.ViewModels;
using System.Transactions;

namespace Artsofte.Data
{
    /// <summary>
    /// This singleton class relies on the ADO.NET framework for performing CRUD operations and provides data access to <see cref="Employee"/>, <see cref="Department"/>, and <see cref="ProgrammingLanguage"/> objects from a MSSQL database through dependency injection services.  
    /// </summary>
    public sealed class ModelsDataAccessLayer : IDisposable
    {
        private string _connectionString;
        private string _pathToSqlQueries;
        private bool _disposed;
        private SqlConnection _sqlConnection = null;

        /// <summary>
        /// A collection of <see cref="Employee"/> objects received from the database.
        /// </summary>
        public IEnumerable<Employee> Employees { get; set; }
        /// <summary>
        ///A collection of <see cref="Department"/> objects received from the database.
        /// </summary>
        public IEnumerable<Department> Departments { get; set; }
        /// <summary>
        /// A collection of <see cref="ProgrammingLanguage"/> objects received from the database.
        /// </summary>
        public IEnumerable<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public bool IsDBExist { get; set; }

        /// <summary>
        /// This class utilizes a lazy instance for dependency injection.
        /// </summary>
        private static Lazy<ModelsDataAccessLayer> instance = new Lazy<ModelsDataAccessLayer>(() => new ModelsDataAccessLayer());

        /// <summary>
        /// Gets the singleton instance of the <see cref="ModelsDataAccessLayer"/> class.
        /// </summary>
        public static ModelsDataAccessLayer Instance => instance.Value;

        /// <summary>
        /// The private constructor sets the connection string to the localDB and the path to the folder with saved SQL procedures.
        /// </summary>
        private ModelsDataAccessLayer()
        {
            _connectionString = $@"Server=(localdb)\mssqllocaldb;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true";
            _pathToSqlQueries = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql_queries");
            //Before checking = state doesn't exist
            IsDBExist = false;
        }
        /// <summary>
        /// Opens an asynchronous connection to the MSSQL database using the <see cref="SqlConnection"/> class.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task OpenConnectionAsync()
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;
            _sqlConnection = new SqlConnection()
            {
                ConnectionString = _connectionString
            };
            if (_sqlConnection.State != ConnectionState.Open)
            {
                await _sqlConnection.OpenAsync();
            }
        }
        /// <summary>
        /// Closing the connection to the MSSQL database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CloseConnectionAsync()
        {
            if (_sqlConnection.State != ConnectionState.Closed && _sqlConnection != null)
            {
                await _sqlConnection.CloseAsync();
            }
        }
        /// <summary>
        /// Checks the existence of the <see cref="SqlConnection"/>. If it exists, the method disposes of the <see cref="SqlConnection"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task DisposeAsync(bool disposing)
        {
            if (_disposed) return;
            if (disposing) await _sqlConnection.DisposeAsync();
            _disposed = true;
        }
        /// <summary>
        /// Realize Disposable method for disposing this class. In a asynchronously process
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Realize IDisposable method for disposing this class. In a synchronously process
        /// </summary>
        public void Dispose()
        {
            DisposeAsync(true).RunSynchronously();
            GC.SuppressFinalize(this);
        }
        //All methods (below) for CRUD processes use the ADO.NET technology
        #region CRUD Operations

        #region Checking and Creation DB
        /// <summary>
        /// Checks the status of the local database and initializes the model properties if it exists,
        /// otherwise creates and fills the database, then initializes the model properties.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CheckDataBaseStatusAsync()
        {
            if (await IsTheLocalDBExistsAsync())
            {
                // The code checks if the local database exists, and if it does,
                // initializes the properties with data from the database and sets IsDBExist to true.
                Employees = await GetAllEmployeesDataAsync();
                Departments = await GetDepartmentsDataAsync();
                ProgrammingLanguages = await GetProgrammingLanguagesDataAsync();
                IsDBExist = true;
                return;
            }
            else
            {
                //if the database doesn't exist, the next creates database and sructure
                //and then fills it tests objects
                //in finally block initialize colletions property
                try
                {
                    await CreationDBAsync();
                    await FillDataBaseAsync();
                    IsDBExist = true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Employees = await GetAllEmployeesDataAsync();
                    Departments = await GetDepartmentsDataAsync();
                    ProgrammingLanguages = await GetProgrammingLanguagesDataAsync();
                }
            }
        }
        /// <summary>
        /// Checks whether the local database exists or not.
        /// </summary>
        /// <returns>A <see cref="Boolean"/> value indicating whether the database exists or not.</returns>
        /// <remarks>This method executes a SQL script to check if the local database exists. If it does, the method returns true, otherwise false.</remarks>
        private async Task<bool> IsTheLocalDBExistsAsync()
        {
            await OpenConnectionAsync();
            //use local folder for received SQL queries
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "CheckDataBaseForExisting.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return ((string)reader["name"]).Equals("Artsofte.Data");//The query must return full name of database [Artsofte.Data]
                    }
                }
            }
            await CloseConnectionAsync();

            return false;
        }
        /// <summary>
        /// Creates the Artsofte database structure by executing SQL scripts stored in a local folder.
        /// Uses the Stack collection to makes the database by querys queue
        /// </summary>
        /// <returns> A task representing the asynchronous operation.</returns>
        private async Task CreationDBAsync()
        {
            var queriesStack = new Stack<string>();
            //Pushing the SQL scripts in a specified sequence
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "CreateEmployeeTable.sql"), Encoding.GetEncoding(1251)));
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "CreateDepartmentsTable.sql"), Encoding.GetEncoding(1251)));
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "CreateProgrammingLanguagesTable.sql"), Encoding.GetEncoding(1251)));
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "ArtsofteCreationDB.sql"), Encoding.GetEncoding(1251)));

            await OpenConnectionAsync();
            do
            {
                using (SqlCommand command = new SqlCommand(queriesStack.Pop(), _sqlConnection))
                {
                    //If there are problems during a transaction, all changes will be rolled back.
                    SqlTransaction transaction = null;
                    try
                    {
                        transaction = _sqlConnection.BeginTransaction();
                        command.Transaction = transaction;
                        command.CommandType = CommandType.Text;
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        await Console.Out.WriteLineAsync(ex.Message);
                        try
                        {
                            //Try to roll back all changes
                            await transaction?.RollbackAsync();
                        }
                        catch (TransactionException ex2)
                        {
                            await Console.Out.WriteLineAsync(ex2.Message);
                            throw;
                        }
                    }
                    finally
                    {
                        //If the transaction is successful, all changes will be committed to the database.
                        await transaction.CommitAsync();
                    }
                }
            } while (queriesStack.Count > 0);
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Fills the database with test data.
        /// Uses the Stack collection to fills the database by querys queue.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FillDataBaseAsync()
        {
            var queriesStack = new Stack<string>();
            //Pushing the SQL scripts in a specified sequence
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "AddTestEmployees.sql"), Encoding.GetEncoding(1251)));
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "AddTestDepartments.sql"), Encoding.GetEncoding(1251)));
            queriesStack.Push(await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "AddTestLanguages.sql"), Encoding.GetEncoding(1251)));

            await OpenConnectionAsync();
            //Uses sequential execution of saved strings and adds them in a while loop.
            do
            {
                using (SqlCommand command = new SqlCommand(queriesStack.Pop(), _sqlConnection))
                {
                    //If there are problems during a transaction, all changes will be rolled back.
                    SqlTransaction transaction = null;
                    try
                    {
                        transaction = _sqlConnection.BeginTransaction();
                        command.Transaction = transaction;
                        command.CommandType = CommandType.Text;
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {                        
                        await Console.Out.WriteLineAsync(ex.Message);
                        try
                        {
                            //Try to roll back all changes
                            await transaction?.RollbackAsync();
                        }
                        catch(TransactionException ex2)
                        {
                            await Console.Out.WriteLineAsync(ex2.Message);
                            throw;
                        }
                    }
                    finally
                    {
                        //If the transaction is successful, all changes will be committed to the database.
                        await transaction.CommitAsync();
                    }
                }
            } while (queriesStack.Count > 0);
            await CloseConnectionAsync();
        }
        #endregion

        #region Read
        /// <summary>
        /// Retrieves all <see cref="Employee"/> objects from the database asynchronously, and binds <see cref="Department"/> and <see cref="ProgrammingLanguage"/> instances for each <see cref="Employee"/>. 
        /// If the connection to the database  doesn't exist, returns an empty <see cref="List{T}"/> of <see cref="Employee"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Employee"/></returns>
        private async Task<IEnumerable<Employee>> GetAllEmployeesDataAsync()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new List<Employee>();

            var employeesList = new List<Employee>();
            //Uses a received SQL script in a local folder
            string sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "GetEmployeesData.sql"), Encoding.GetEncoding(1251));

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employeesList.Add(new Employee()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Surname = (string)reader["Surname"],
                            Age = (int)reader["Age"],
                            Gender = (string)reader["Gender"],
                            DepartmentId = (int)reader["DepartmentId"],
                            ProgrammingLanguageId = (int)reader["ProgrammingLanguageId"],
                            //Binding departmnet and prog langs data by inner join in a sql script
                            Department = new Department() { Id = (int)reader["DepartmentId"], Name = (string)reader["deptName"], Floor = (string)reader["deptFloor"] },
                            ProgrammingLanguage = new ProgrammingLanguage() { Id = (int)reader["ProgrammingLanguageId"], Name = (string)reader["ProgLangName"] }
                        });
                    }
                }
            }

            await CloseConnectionAsync();
            return employeesList;
        }
        /// <summary>
        /// Retrieves all <see cref="Department"/> data from the database.
        /// If the connection to the database  doesn't exist, returns an empty <see cref="List{T}"/> of <see cref="Department"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Department"/> objects representing the retrieved data.</returns>
        private async Task<IEnumerable<Department>> GetDepartmentsDataAsync()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new List<Department>();
            var departmentsList = new List<Department>();
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "GetDepartmentsData.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        departmentsList.Add(new Department()
                        {
                            Id = (int)reader["ID"],
                            Name = (string)reader["Name"],
                            Floor = (string)reader["Floor"]

                        });
                    }
                }
            }
            await CloseConnectionAsync();
            return departmentsList;
        }
        /// <summary>
        /// Retrieves all <see cref="ProgrammingLanguage"/> data from the database.
        /// If the connection to the database  doesn't exist, returns an empty <see cref="List{T}"/> of <see cref="ProgrammingLanguage"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="ProgrammingLanguage"/> objects representing the retrieved data.</returns>
        private async Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesDataAsync()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new Collection<ProgrammingLanguage>();
            var progLangsList = new Collection<ProgrammingLanguage>();
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "GetProgrammingLnguagesData.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        progLangsList.Add(new ProgrammingLanguage()
                        {
                            Id = (int)reader["ID"],
                            Name = (string)reader["Name"]

                        });
                    }
                }
            }
            await CloseConnectionAsync();
            return progLangsList;

        }
        #endregion

        #region Create
        /// <summary>
        /// Inserts a new <see cref="Employee"/> record into the database using the provided <see cref="EmployeeVM"/> object.        
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee"> The <see cref="EmployeeVM"/> object to be inserted into the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InsertEmployee(EmployeeVM employee)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "InsertEmployee.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" (N'{employee.Name}', N'{employee.Surname}', {employee.Age}, N'{employee.Gender}', {employee.DepartmentId}, {employee.ProgrammingLanguageId})";
            //Concats strings script + values to single query
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from DB after update.
                    Employees = await GetAllEmployeesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }

        /// <summary>
        /// Inserts a new <see cref="Department"/> record into the database using the provided <see cref="DepartmentVM"/> object.        
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee"> The <see cref="DepartmentVM"/> object to be inserted into the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InsertDepartmentAsync(DepartmentVM department)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "InsertDepartment.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" (N'{department.Name}', N'{department.Floor}')";
            //Concats strings script + values to single query
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    transaction?.Commit();
                    //Refresh data from the DB
                    Departments = await GetDepartmentsDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Inserts a new <see cref="ProgrammingLanguage"/> record into the database using the provided <see cref="ProgrammingLanguageVM"/> object.        
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee"> The <see cref="ProgrammingLanguage"/> object to be inserted into the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InsertProgrammingLanguages(ProgrammingLanguageVM languageVM)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "InsertProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" (N'{languageVM.Name}')";
            //Concats strings script + values to single query
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from the DB
                    ProgrammingLanguages = await GetProgrammingLanguagesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes a record of the <see cref="Employee"/> from the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteEmployeeAsync(Employee employee)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "DeleteEmployee.sql"), Encoding.GetEncoding(1251));
            // Deleting a record from database by Id
            using (SqlCommand command = new SqlCommand(sql + employee.Id.ToString(), _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from DB after update.
                    Employees = await GetAllEmployeesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Deletes a record of the <see cref="Department"/> from the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee">The <see cref="Department"/> to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteDepartmentAsync(Department department)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "DeleteDepartment.sql"), Encoding.GetEncoding(1251));
            // Deleting a record from database by Id
            using (SqlCommand command = new SqlCommand(sql + department.Id.ToString(), _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from the DB
                    Departments = await GetDepartmentsDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Deletes a record of the <see cref="ProgrammingLanguage"/> from the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee">The <see cref="ProgrammingLanguage"/> to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteProgrammingLanguageAsync(ProgrammingLanguage language)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "DeleteProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            // Deleting a record from database by Id
            using (SqlCommand command = new SqlCommand(sql + language.Id.ToString(), _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    // Refresh data from the DB
                    ProgrammingLanguages = await GetProgrammingLanguagesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the data of an existing <see cref="Employee"/> in the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "UpdateEmployee.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" Name = N'{employee.Name}', Surname = N'{employee.Surname}', Age = {employee.Age}, Gender = N'{employee.Gender}', DepartmentId = {employee.DepartmentId}, ProgrammingLanguageId = {employee.ProgrammingLanguageId}";
            // Constructs a complete SQL query string, including all values and the Id key for the WHERE condition, using the string.Format() method.
            var fullQuery = string.Format(sql, values, employee.Id);
            using (SqlCommand command = new SqlCommand(fullQuery, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteReaderAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from DB after update.
                    Employees = await GetAllEmployeesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Updates the data of an existing <see cref="Department"/> in the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="department">The <see cref="Department"/> to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateDepartmentAsync(Department department)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "UpdateDepartment.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" Name = N'{department.Name}', Floor =  N'{department.Floor}'";
            // Constructs a complete SQL query string, including all values and the Id key for the WHERE condition, using the string.Format() method.
            var fullQuery = string.Format(sql, values, department.Id);
            using (SqlCommand command = new SqlCommand(fullQuery, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from DB after update.
                    Departments = await GetDepartmentsDataAsync();
                }
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Updates the data of an existing <see cref="ProgrammingLanguage"/> in the database.
        /// If there are problems during a transaction, all changes will be rolled back.If the transaction is successful, all changes will be committed to the database.
        /// Finally, refreshes data from the database after update.
        /// </summary>
        /// <param name="language">The <see cref="ProgrammingLanguage"/> to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateProgrammingLanguagesAsync(ProgrammingLanguage language)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            //Uses a received SQL script in a local folder
            var sql = await @File.ReadAllTextAsync(Path.Combine(_pathToSqlQueries, "UpdateProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            // Adds values to the SQL script
            var values = $@" Name = N'{language.Name}'";
            // Constructs a complete SQL query string, including all values and the Id key for the WHERE condition, using the string.Format() method.
            var fullQuery = string.Format(sql, values, language.Id);
            using (SqlCommand command = new SqlCommand(fullQuery, _sqlConnection))
            {
                //If there are problems during a transaction, all changes will be rolled back.
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    try
                    {
                        //Try to roll back all changes
                        await transaction?.RollbackAsync();
                    }
                    catch (TransactionException ex2)
                    {
                        await Console.Out.WriteLineAsync(ex2.Message);
                        throw;
                    }
                }
                finally
                {
                    //If the transaction is successful, all changes will be committed to the database.
                    await transaction?.CommitAsync();
                    //Refresh data from DB after update.
                    ProgrammingLanguages = await GetProgrammingLanguagesDataAsync();
                }
            }
            await CloseConnectionAsync();
        }


    }
    #endregion

    #endregion
}
