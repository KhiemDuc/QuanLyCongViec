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
    public class InfoUserDAL: DatabaseAccess
    {
        public async Task<string> GetNickNameAsync(string id)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = Connect();

                await OpenConAsync();
                myCommand.CommandText = String.Format($"select nickname " +
                                                      $"from tInfoUser " +
                                                      $"where user_id = @id");
                myCommand.Parameters.AddWithValue("@id", id);

                var nickname = await myCommand.ExecuteScalarAsync();

                if (nickname != null)
                    return nickname.ToString();
                else
                    return "default";

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

        public async Task<bool> UpdateInfoUser(string id, string fullname, DateTime dateobirth, byte sex, string nickname,
            CancellationToken cancellation)
        {
            byte sexBit = sex > 0 ? (byte)1 : (byte)0;

            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                cancellation.ThrowIfCancellationRequested();

                sqlCommand.Connection = Connect();
                await OpenConAsync(cancellation);

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = String.Format($"Update [tInfoUser]  SET  " +
                                                        $"[fullname] = @fullname, " +
                                                        $"[date_of_birth] = @dateobirth, " +
                                                        $"[sex] = @sex, " +
                                                        $"[nickname] = @nickname " +
                                                        $"WHERE [user_id] = @id");

                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@fullname", fullname);
                sqlCommand.Parameters.AddWithValue("@dateobirth", dateobirth);
                sqlCommand.Parameters.AddWithValue("@sex", sexBit);
                sqlCommand.Parameters.AddWithValue("@nickname", nickname);


                var rowAffected = await sqlCommand.ExecuteNonQueryAsync(cancellation);

                if (rowAffected != 0)
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

        public async Task<Dictionary<string,object>> GetInfoUserAsync(string id)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = Connect();

                await OpenConAsync();
                myCommand.CommandText = String.Format($"select * " +
                                                      $"from tInfoUser " +
                                                      $"where user_id = @id");
                myCommand.Parameters.AddWithValue("@id", id);

                var reader = await myCommand.ExecuteReaderAsync();

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
    }
}
