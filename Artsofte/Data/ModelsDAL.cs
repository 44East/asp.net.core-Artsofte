﻿using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System;
using Artsofte.Models;
using System.Text;
using Artsofte.Models.ViewModels;

namespace Artsofte.Data
{
    public class ModelsDAL : IDisposable
    {
        private string _connectionString;
        private string _pathToSqlQueries;
        private bool _disposed;
        private SqlConnection _sqlConnection = null;
        private bool _dbDataExists;

        public IQueryable<Employee> Employees { get; set; }
        public IQueryable<Department> Departments { get; set; }
        public IQueryable<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public ModelsDAL()
        {
            _connectionString = $@"Server=(localdb)\mssqllocaldb;Database=Artsofte.Data;Trusted_Connection=True;MultipleActiveResultSets=true";
            _pathToSqlQueries = Path.Combine(Environment.CurrentDirectory, "sql_queries");
            _dbDataExists = IsTheLocalDBExistsAsync();
            if (!_dbDataExists)//If the DB doesn't exist, will be create a new test DB with the test data.
                CreationDB().RunSynchronously();
            Employees = GetAllEmployeesData().Result.AsQueryable();


        }
        /// <summary>
        /// Connect to the MSSQL DB
        /// </summary>
        private async Task OpenConnectionAsync()
        {
            if (_connectionString == null)
                return;
            _sqlConnection = new SqlConnection()
            {
                ConnectionString = _connectionString
            };
            await _sqlConnection.OpenAsync();
        }
        /// <summary>
        /// Closing connection to the MSSQL DB 
        /// </summary>
        private async Task CloseConnectionAsync()
        {
            if (_sqlConnection.State != ConnectionState.Closed)
            {
                await _sqlConnection.CloseAsync();
            }
        }
        /// <summary>
        /// Checks the SqlConnection to exist, if it exists, method get rid of the SqlConnection
        /// </summary>
        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (_disposed) return;
            if (disposing) await _sqlConnection.DisposeAsync();
            _disposed = true;
        }
        /// <summary>
        /// Disposing of the SqlConnection and Finalize [this.instance] 
        /// </summary>
        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose()
        {
            DisposeAsync(true).RunSynchronously();
            GC.SuppressFinalize(this);
        }
        //All methods (below) for CRUD processes use the ADO.NET technology
        #region CRUD Operations

        /// <summary>
        /// Checks the localhost MSSQL Server DB for an availability saved database [ToDoList]
        /// It sends query to the DB for receiving an Id number of one the tables from the DB.
        /// If the DB returns null, will be create a new test DB with the test data.
        /// </summary>
        private async Task<bool> IsTheLocalDBExistsAsync()
        {
            await OpenConnectionAsync();
            var isDBExists = false;
            var sql = $@"SELECT OBJECT_ID (N'ToDoList.dbo.Persons', N'U') AS 'ID'";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        isDBExists = reader["ID"] is System.Int32;
                    }
                }
            }
            await CloseConnectionAsync();

            return isDBExists;
        }
        /// <summary>
        /// Method for filling the DB of the testing data. 
        /// It uses the saved qureies from the special class [CreationLocalDBSqlQuerys].
        /// Requests are sent to the database in turn using Stack.Pop()
        /// </summary>
        private async Task CreationDB()
        {
            var queriesStack = new Stack<string>();

            await OpenConnectionAsync();
            do
            {
                using (SqlCommand command = new SqlCommand(queriesStack.Pop(), _sqlConnection))
                {
                    try
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Exception error = new Exception("LocalBD cann't fill!", ex);
                        throw error;
                    }
                }
            } while (queriesStack.Count > 0);
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Returns the ToDoTasks collection from the DB and it includes a Persons names for the ToDoTask model.
        /// </summary>
        public async Task<IEnumerable<Employee>> GetAllEmployeesData()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new Collection<Employee>();

            var employeesList = new List<Employee>();
            string sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "GetEmployeesData.sql"), Encoding.GetEncoding(1251));

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
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
                            Department = new Department() { Id = (int)reader["DepartmentId"], Name = (string)reader["deptName"], Floor = (string)reader["deptFloor"] },
                            ProgrammingLanguage = new ProgrammingLanguage() { Id = (int)reader["ProgrammingLanguageId"], Name = (string)reader["ProgLangName"] }
                        }
                        );
                    }
                }
            }

            await CloseConnectionAsync();
            return employeesList;
        }
        /// <summary>
        /// It returns the Persons collection from the DB.
        /// </summary>
        public async Task<IEnumerable<Department>> GetDepartmentsData()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new Collection<Department>();
            var departmentsList = new Collection<Department>();
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "GetDepartmentsData.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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

        public async Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesData()
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return new Collection<ProgrammingLanguage>();
            var progLangsList = new Collection<ProgrammingLanguage>();
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "GetProgrammingLnguagesData.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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
        /// <summary>
        /// It method insert into DB NEW ToDoTask.
        /// It receives params of the Person and the ToDoTask then it will create dependences between the Person table and the Task table
        /// </summary>
        /// <param name="description"></param>
        /// <param name="taskName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public async Task InsertEmployee(EmployeeVM employee)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "InsertEmployee.sql"), Encoding.GetEncoding(1251));
            var values = $@" (N'{employee.Name}', N'{employee.Surname}', {employee.Age}, N'{employee.Gender}', {employee.DepartmentId}, {employee.ProgrammingLanguageId})";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Add NEW Person in the Person table into DB
        /// </summary>
        /// <param name="person"></param>
        public async Task InsertDepartment(DepartmentVM department)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "InsertDepartment.sql"), Encoding.GetEncoding(1251));
            var values = $@" (N'{department.Name}', N'{department.Floor}')";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }

        public async Task InsertProgrammingLanguages(ProgrammingLanguageVM languageVM)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "InsertProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            var values = $@" (N'{languageVM.Name}')";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Delete the ToDoTask in the DB by [ID] params
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteEmployee(int id)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "DeleteEmployee.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql + id.ToString(), _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
            }
            await CloseConnectionAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "DeleteDepartment.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql + id.ToString(), _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
            }
            await CloseConnectionAsync();
        }

        public async Task DeleteProgrammingLanguage(int id)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "DeleteProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            using (SqlCommand command = new SqlCommand(sql + id.ToString(), _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
            }
            await CloseConnectionAsync();
        }

        /// <summary>
        /// Update the exist ToDoTask into DB, It updates only a [Description] field
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="taskName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void UpdateTask(int id, string description, string taskName, string firstName, string lastName)
        {
            OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = $@"USE ToDoList DECLARE @PersonID INT
                         SELECT @PersonID = i.ID FROM Persons i WHERE i.First_Name = N'{firstName}' and i.Last_Name = N'{lastName}'
                         UPDATE Tasks SET Description = N'{description}', Assigned_Person = @PersonID, Name = N'{taskName}'  WHERE ID = {id}";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("The operation couldn’t be completed!", ex);
                    throw error;
                }
            }
            CloseConnectionAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "UpdateEmployee.sql"), Encoding.GetEncoding(1251));
            var values = $@" (Name = N'{employee.Name}', Surname = N'{employee.Surname}', Age = {employee.Age}, Gender = N'{employee.Gender}', DepartmentId = {employee.DepartmentId}, ProgrammingLanguage = {employee.ProgrammingLanguageId})";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }
        /// <summary>
        /// Add NEW Person in the Person table into DB
        /// </summary>
        /// <param name="person"></param>
        public async Task InsertDepartment(DepartmentVM department)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "InsertDepartment.sql"), Encoding.GetEncoding(1251));
            var values = $@" (N'{department.Name}', N'{department.Floor}')";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }

        public async Task InsertProgrammingLanguages(ProgrammingLanguageVM languageVM)
        {
            await OpenConnectionAsync();
            if (_sqlConnection == null)
                return;
            var sql = @File.ReadAllText(Path.Combine(_pathToSqlQueries, "InsertProgrammingLanguage.sql"), Encoding.GetEncoding(1251));
            var values = $@" (N'{languageVM.Name}')";
            using (SqlCommand command = new SqlCommand(sql + values, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    await Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
                transaction?.Commit();
            }
            await CloseConnectionAsync();
        }


    }
    #endregion
}