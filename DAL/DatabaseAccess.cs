
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class DatabaseAccess
    {
        private SqlConnection conn;
        protected SqlConnection Connect()
        {
            string connString = @"Data Source=LAPTOP-KKEAO781\SQLEXPRESS;Initial Catalog=Todo;Integrated Security=True";
            conn = new SqlConnection(connString);
            return conn;
        }
        protected async Task<SqlConnection> OpenConAsync(CancellationToken cancellation = default)
        {
            if (conn != null && (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken))
            {
                await conn.OpenAsync(cancellation);
            }
            return conn;
        }
        protected SqlConnection CloseConnect()
        {
            if (conn != null  && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return conn;
        }
        public void Dispose()
        {
            conn?.Dispose();
        }
    }
}
