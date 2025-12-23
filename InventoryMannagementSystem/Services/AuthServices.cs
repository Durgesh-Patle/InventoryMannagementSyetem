using System.Data;

using InventoryMannagementSystem.Common;

using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace InventoryMannagementSystem.Services
{
    public class AuthServices : IAuth
    {
        private readonly string _connectionString;

        public AuthServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponceModel> InsertRegisterUser(AuthModel user)
        {
            var res = new ResponceModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_register_user", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", user.Name);
                        cmd.Parameters.AddWithValue("@email", user.Email);

                        string hashedPassword = AesGenerator.Encrypt(user.Password);

                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        var outputParam = new SqlParameter
                        {
                            ParameterName = "@ReturnMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 100,
                            Direction = ParameterDirection.Output,
                        };
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        res.Status = true;
                        res.Message = outputParam.Value.ToString();
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "Error while inserting user.";
                Console.WriteLine($"Error in InsertRegisterUser: {ex.Message}");
                Helper.WriteLog($"Error in InsertRegisterUser: {ex.Message}");
                return res;
            }
        }

        public async Task<ResponceModel> ValidateUser(AuthModel user)
        {
            var res = new ResponceModel();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_validate_user", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@email", user.Email);

                string hashedPassword = AesGenerator.Encrypt(user.Password);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                var outputParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                res.Status = outputParam.Value.ToString() == "Login successful";
                res.Message = outputParam.Value.ToString();

            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.ToString());
                res.Status = false;
                res.Message = "Something went wrong during login";
            }

            return res;
        }
        public async Task<List<AuthModel>> GetUserDetails(AuthModel user)
        {
            var users = new List<AuthModel>();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_get_user_details_by_email", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", user.Email);

                await conn.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new AuthModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Email = reader.GetString(reader.GetOrdinal("email"))
                    });
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.ToString());
            }

            return users;
        }


    }
}
