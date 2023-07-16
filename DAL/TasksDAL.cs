using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DAL
{
    public class TasksDAL:DatabaseAccess
    {
        // Task
        public async Task<DataTable> GetDataTableTodoAsync(string user_id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();
                await OpenConAsync();
                    
                sqlCommand.CommandText = string.Format($"SELECT  t.task_id, t.task_name, t.task_des, t.task_status," +
                                            $"FORMAT (t.start_date, 'dd-MM-yyyy HH:mm') as start_date, " +
                                            $"FORMAT (t.end_date, 'dd-MM-yyyy HH:mm') as end_date, " +
                                            $"FORMAT (t.date_done, 'dd-MM-yyyy HH:mm') as date_done, g.group_name " +
                                            "FROM tTasks AS t INNER JOIN tGroup AS g ON t.group_id = g.group_id " +
                                            "WHERE t.user_id = @user_id");
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);
                adapter.SelectCommand = sqlCommand;

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

        public async Task<DataTable> FindDataTableTodoAsync(string user_id, string dayStart,DateTime dayEnd, string status)
        {

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();
                await OpenConAsync();

                sqlCommand.CommandText = string.Format($"SELECT  t.task_id, t.task_name, t.task_des, t.task_status," +
                                            $"FORMAT (t.start_date, 'dd-MM-yyyy HH:mm') as start_date, " +
                                            $"FORMAT (t.end_date, 'dd-MM-yyyy HH:mm') as end_date, " +
                                            $"FORMAT (t.date_done, 'dd-MM-yyyy HH:mm') as date_done, g.group_name " +
                                            $"FROM tTasks AS t INNER JOIN tGroup AS g ON t.group_id = g.group_id " +
                                            $"WHERE t.user_id = @user_id and " +
                                            $"(CASE WHEN @dayStart = 'start_date' THEN t.start_date ELSE t.end_date END) <= @dayEnd " +
                                            $"and t.task_status<>@status");
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);
                sqlCommand.Parameters.AddWithValue("@dayStart", dayStart);
                sqlCommand.Parameters.AddWithValue("@status", status);
                sqlCommand.Parameters.AddWithValue("@dayEnd", dayEnd);


                adapter.SelectCommand = sqlCommand;

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

        public async Task<Dictionary<string, object>> GetInfoTodoAsync(string task_id)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();
                await OpenConAsync();

                sqlCommand.CommandText = string.Format($"SELECT  t.task_name, t.task_des, t.task_status," +
                                                       $"t.start_date,t.end_date, g.group_name " +
                                                       "FROM tTasks AS t INNER JOIN tGroup AS g ON t.group_id = g.group_id " +
                                                       "WHERE t.task_id = @task_id");
                sqlCommand.Parameters.AddWithValue("@task_id", task_id);
                var reader = await sqlCommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var dict = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dict.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    return dict;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                CloseConnect();
                sqlCommand.Dispose();
            }
        }

        // Action
        public async Task<bool> InsertTodo(string task_name,string group_id, string task_des, DateTime start_date,
            DateTime end_date, string status, string user_id, CancellationToken cancellation)
        {
           

            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();

                await OpenConAsync(cancellation);

                sqlCommand.CommandText = String.Format($"INSERT INTO [tTasks] ([group_id],[task_name],[task_des]," +
                    $"[start_date],[end_date],[task_status],[user_id],[date_done]) " +
                    $"VALUES(@group_id,@task_name,@task_des,@start_date,@end_date,@task_status,@user_id,@date_done)");

                sqlCommand.Parameters.AddWithValue("@task_name", task_name);
                sqlCommand.Parameters.AddWithValue("@task_des", task_des);
                sqlCommand.Parameters.AddWithValue("@start_date", start_date);
                sqlCommand.Parameters.AddWithValue("@end_date", end_date);
                sqlCommand.Parameters.AddWithValue("@task_status", status);
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);
                sqlCommand.Parameters.AddWithValue("@group_id", group_id);

                if (status == "Completed")
                {
                    sqlCommand.Parameters.AddWithValue("@date_done", DateTime.Now);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@date_done", DBNull.Value);
                }

                var rowEfected = await sqlCommand.ExecuteNonQueryAsync(cancellation);
                return (rowEfected != 0);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnect();
                sqlCommand.Dispose();

            }

        }

        public async Task DeleteToDo(string task_id)
        {
                SqlCommand sqlCommand = new SqlCommand();
                try
                {
                    sqlCommand.Connection = Connect();
                    await OpenConAsync();
                    sqlCommand.CommandText = String.Format($"DELETE FROM [tTasks] WHERE task_id = @task_id");
                    sqlCommand.Parameters.AddWithValue("@task_id", task_id);
                    sqlCommand.ExecuteNonQuery();
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

        public async Task<bool> UpdateTodo(string task_id, string task_name, string group_id, string task_des, DateTime start_date,
            DateTime end_date, string status, CancellationToken cancellation)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = Connect();

                await OpenConAsync(cancellation);

                sqlCommand.CommandText = String.Format($"UPDATE tTasks SET task_name = @task_name," +
                                                                        $"task_des = @task_des," +
                                                                        $"start_date = @start_date," +
                                                                        $"end_date = @end_date, " +
                                                                        $"task_status = @task_status, " +
                                                                        $"group_id = @group_id," +
                                                                        $"date_done = @date_done " +
                                                                        $"WHERE task_id = @task_id");

                sqlCommand.Parameters.AddWithValue("@task_name", task_name);
                sqlCommand.Parameters.AddWithValue("@task_des", task_des);
                sqlCommand.Parameters.AddWithValue("@start_date", start_date);
                sqlCommand.Parameters.AddWithValue("@end_date", end_date);
                sqlCommand.Parameters.AddWithValue("@task_status", status);
                sqlCommand.Parameters.AddWithValue("@task_id", task_id);
                sqlCommand.Parameters.AddWithValue("@group_id", group_id);

                if (status == "Completed")
                {
                    sqlCommand.Parameters.AddWithValue("@date_done", DateTime.Now);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@date_done", DBNull.Value);
                }

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
