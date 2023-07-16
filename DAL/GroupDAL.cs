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
    public class GroupDAL: DatabaseAccess
    {
        // Group
        public async Task<DataTable> GetGroupNameAsync(string user_id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();

                await OpenConAsync();

                sqlCommand.CommandText = String.Format($"select group_id, group_name from tGroup where user_id = @user_id");

                sqlCommand.Parameters.AddWithValue("@user_id", user_id);

                adapter.SelectCommand = sqlCommand;
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
            finally
            {
                CloseConnect();
                sqlCommand.Dispose();
                adapter.Dispose();
            }

        }
        public async Task<bool> CheckGroupNameAsync(string user_id, string group_name, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();

                await OpenConAsync();

                sqlCommand.CommandText = String.Format($"select group_id, group_name from tGroup " +
                    $"where user_id = @user_id and group_name = @group_name");

                sqlCommand.Parameters.AddWithValue("@user_id", user_id);
                sqlCommand.Parameters.AddWithValue("@group_name", group_name);


                var reader = await sqlCommand.ExecuteReaderAsync(cancellation);
                if (reader.HasRows)
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnect();
                sqlCommand.Dispose();

            }
        }

        //Action
        public async Task<bool> InsertGroup(string user_id, string group_name, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();

                await OpenConAsync(cancellation);

                sqlCommand.CommandText = String.Format($"INSERT INTO [tGroup] ([group_name],[user_id]) " +
                    $"VALUES(@group_name,@user_id)");

                sqlCommand.Parameters.AddWithValue("@user_id", user_id);
                sqlCommand.Parameters.AddWithValue("@group_name", group_name);

                var rowEffected = await sqlCommand.ExecuteNonQueryAsync(cancellation);
                return (rowEffected != 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnect();
                sqlCommand.Dispose();
            }

        }

    }
}
