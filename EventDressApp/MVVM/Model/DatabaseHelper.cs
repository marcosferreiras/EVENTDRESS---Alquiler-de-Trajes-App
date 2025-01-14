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
        private readonly string _connectionString = "Server=MARCOS-FERREIRA;Database=EVENTDRESS;Trusted_Connection=True; Encrypt=False;";

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

        private DatabaseHelper()
        {
            _connection = new SqlConnection(_connectionString);
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        // Execute a SELECT query and return a DataTable
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.CommandType = commandType;
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

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.CommandType = commandType;
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

        public int ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
        {
            return ExecuteNonQuery(storedProcedureName, parameters, CommandType.StoredProcedure);
        }

        // Método corregido para ejecutar stored procedures que retornan resultados
        public DataTable ExecuteStoredProcedureWithResults(string storedProcedureName, SqlParameter[] parameters = null)
        {
            return ExecuteQuery(storedProcedureName, parameters, CommandType.StoredProcedure);
        }

        public DataTable GetAllRecords(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";
            return ExecuteQuery(query);
        }

        public List<T> ExecuteQueryToList<T>(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                DataTable dataTable = ExecuteQuery(query, parameters, commandType);
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