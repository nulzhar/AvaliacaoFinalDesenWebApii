using System;
using System.Data;
using System.Data.SqlClient;

namespace AvaliacaoFinalDesenWebApii.Models
{
    public class DbConnection : IDisposable
    {
        SqlConnection conn;

        public DbConnection(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public DataTable GetList(string query)
        {
            DataTable dataTable = new DataTable();

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            da.Dispose();

            return dataTable;
        }

        public DataTable Insert(string insertCmd)
        {
            DataTable dataTable = new DataTable();

            SqlCommand cmd = new SqlCommand(insertCmd, conn);
            conn.Open();

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            return dataTable;
        }

        public void Dispose()
        {
            conn.Close();
        }
    }
}
