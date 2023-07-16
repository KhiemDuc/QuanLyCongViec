
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace DAL
{
    public class UserAccessDAL : DatabaseAccess
    {
        public async Task<bool> CheckUserAsync(string username, CancellationToken cancellation)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                cancellation.ThrowIfCancellationRequested();
                myCommand.Connection = Connect();
                await OpenConAsync(cancellation);
                myCommand.CommandText = String.Format($"Select * From [tUSER] " +
                                                      $"Where username = @user_name ");

                myCommand.Parameters.AddWithValue("@user_name", username);

                var reader = await myCommand.ExecuteReaderAsync(cancellation);
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnect();
                myCommand.Dispose();
            }

        }

        public async Task<string> LoginAndGetUserIdAsync(string username, string password, CancellationToken cancellation)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                cancellation.ThrowIfCancellationRequested();
                await Task.Delay(3000);

                myCommand.Connection = Connect();
                await OpenConAsync(cancellation);

                myCommand.CommandText = String.Format($"Select * from [tUSER] where username = @user_name and password = @pass_word");
                myCommand.Parameters.AddWithValue("@user_name", username);
                myCommand.Parameters.AddWithValue("@pass_word", password);

                var userID = await myCommand.ExecuteScalarAsync(cancellation);

                if(userID != null)
                    return userID.ToString();
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnect();
                myCommand.Dispose();
            }



        }

        public async Task<bool> InsertUserDALAsync(string username, string password, string email, CancellationToken cancellation)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                cancellation.ThrowIfCancellationRequested();

                sqlCommand.Connection = Connect();
                await OpenConAsync(cancellation);

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = String.Format($"Insert into [tUSER] (username, password, email) " +
                                                       $"values (@user_name, @password, @email)");

                sqlCommand.Parameters.AddWithValue("@user_name", username);
                sqlCommand.Parameters.AddWithValue("@password", password);
                sqlCommand.Parameters.AddWithValue("@email", email);

                var rowAffected = await sqlCommand.ExecuteNonQueryAsync(cancellation);
                return (rowAffected != 0);
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

        public async Task<bool> UpdatePasswordUserAsync(string username, string oldpassword,string newpassword, 
            CancellationToken cancellation)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                cancellation.ThrowIfCancellationRequested();

                sqlCommand.Connection = Connect();
                await OpenConAsync(cancellation);

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = String.Format($"Update [tUSER]  SET  " +
                                                       $"[password] = @new_password " +
                                                       $"WHERE [username] = @user_name and [password] = @old_password ");

                sqlCommand.Parameters.AddWithValue("@user_name", username);
                sqlCommand.Parameters.AddWithValue("@old_password", oldpassword);
                sqlCommand.Parameters.AddWithValue("@new_password", newpassword);


                var rowAffected = await sqlCommand.ExecuteNonQueryAsync(cancellation);

                return (rowAffected != 0);
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

