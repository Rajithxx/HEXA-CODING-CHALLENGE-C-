using Microsoft.Data.SqlClient;
using System;

namespace LoanManagementSystem.util
{
    public class DbUtil
    {
        private static readonly string connectionString = "Server=KAMINARI\\SQLSERVER2022;Database=LoanManagementDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message);
                throw;
            }
        }
    }
}
