using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Base class for database operations using ADO.NET (.NET 2.0 compatible)
    /// </summary>
    public class DatabaseHelper : IDisposable
    {
        private string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AutoparkDBConnection"].ConnectionString;
        }

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get or create database connection
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                return _connection;
            }
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            _transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Execute a non-query SQL command (INSERT, UPDATE, DELETE)
        /// </summary>
        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (SqlCommand command = CreateCommand(commandText, parameters, commandType))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute a SQL command that returns a single value
        /// </summary>
        public object ExecuteScalar(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (SqlCommand command = CreateCommand(commandText, parameters, commandType))
            {
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Execute a SQL command that returns a SqlDataReader
        /// </summary>
        public SqlDataReader ExecuteReader(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            SqlCommand command = CreateCommand(commandText, parameters, commandType);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Execute a SQL command and return a DataTable
        /// </summary>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (SqlCommand command = CreateCommand(commandText, parameters, commandType))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        /// <summary>
        /// Execute a SQL command and return a DataSet
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (SqlCommand command = CreateCommand(commandText, parameters, commandType))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }

        /// <summary>
        /// Create a SqlCommand with the specified parameters
        /// </summary>
        private SqlCommand CreateCommand(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, Connection);
            command.CommandType = commandType;

            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            return command;
        }

        /// <summary>
        /// Dispose pattern implementation
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }

        /// <summary>
        /// Helper method to create SqlParameter
        /// </summary>
        public static SqlParameter CreateParameter(string name, object value, SqlDbType dbType, int size = 0)
        {
            SqlParameter parameter = new SqlParameter(name, dbType);
            parameter.Value = value ?? DBNull.Value;
            
            if (size > 0)
            {
                parameter.Size = size;
            }
            
            return parameter;
        }

        /// <summary>
        /// Helper method to create output SqlParameter
        /// </summary>
        public static SqlParameter CreateOutputParameter(string name, SqlDbType dbType, int size = 0)
        {
            SqlParameter parameter = new SqlParameter(name, dbType);
            parameter.Direction = ParameterDirection.Output;
            
            if (size > 0)
            {
                parameter.Size = size;
            }
            
            return parameter;
        }
    }
}