using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EventDressApp.MVVM.Model
{
    internal class DatabaseHelper
    {
        private static DatabaseHelper _instance;
        private readonly SqlConnection _connection;
        private readonly string _connectionString = "Server=ALBINSON\\MSSQLSERVER01;Database=EVENTDRESS;Trusted_Connection=True; Encrypt=False;";

        // Singleton pattern to ensure a single instance of DatabaseHelper
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseHelper();
                }
                return _instance;
            }
        }

        // Private constructor to prevent external instantiation
        private DatabaseHelper()
        {
            _connection = new SqlConnection(_connectionString);
        }

        // Open database connection
        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        // Close database connection
        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        // Execute a SELECT query and return a DataTable
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query: " + ex.Message, ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Execute a query (INSERT, UPDATE, DELETE) that doesn't return data
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.CommandType = commandType;  // Can be stored procedure or raw SQL
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing non-query: " + ex.Message, ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Execute a stored procedure (with parameters) and return the number of affected rows
        public int ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
        {
            return ExecuteNonQuery(storedProcedureName, parameters, CommandType.StoredProcedure);
        }

        // Execute a stored procedure and return a DataTable (useful for SELECT operations in procedures)
        public DataTable ExecuteStoredProcedureWithResults(string storedProcedureName, SqlParameter[] parameters = null)
        {
            return ExecuteQuery(storedProcedureName, parameters);
        }

        // Retrieve all records from a specific table
        public DataTable GetAllRecords(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";
            return ExecuteQuery(query);
        }

        // A more generic method to execute queries that return a list of custom objects
        public List<T> ExecuteQueryToList<T>(string query, SqlParameter[] parameters = null)
        {
            try
            {
                DataTable dataTable = ExecuteQuery(query, parameters);
                List<T> resultList = new List<T>();

                foreach (DataRow row in dataTable.Rows)
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var property = obj.GetType().GetProperty(column.ColumnName);
                        if (property != null && row[column] != DBNull.Value)
                        {
                            property.SetValue(obj, row[column]);
                        }
                    }
                    resultList.Add(obj);
                }

                return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing query and mapping to list: " + ex.Message, ex);
            }
        }
    }
}
