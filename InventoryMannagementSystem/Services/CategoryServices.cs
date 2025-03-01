using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InventoryMannagementSystem.Services
{
    public class CategoryServices : ICategory
    {
        private readonly string _connectionString;

        public CategoryServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            var categories = new List<Category>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_GetAllCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categories.Add(new Category
                                {
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllEmployeeAsync: {ex.Message}");
            }

            return categories;
        }

        public async Task<string> InsertCategoryAsync(Category cat)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);

                        var outputParam = new SqlParameter
                        {
                            ParameterName = "@ReturnMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 100,
                            Direction = ParameterDirection.Output,
                        };
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        return outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InsertEmployeeAsync: {ex.Message}");
                return "Error occurred while inserting employee";
            }
        }

        public async Task<string> DeleteCategoryAsync(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_DeleteCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CategoryId", id);

                        var outputParan = new SqlParameter(
                            "@ReturnMessage",
                            SqlDbType.NVarChar,
                            100)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputParan);

                        await cmd.ExecuteNonQueryAsync();

                        string returnValue = outputParan.Value.ToString();

                        return returnValue;
                    }

                }
            }
            catch (Exception ex)
            {
                return $"error: {ex.Message}";
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            Category cat = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_GetCategoryById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CategoryId", id);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                cat = new Category
                                {
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching category with ID {id}: {ex.Message}");
            }
            return cat;
        }
        public async Task<string> UpdateCategoryAsync(Category cat)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateCategoryById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CategoryId", cat.CategoryId);
                        cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);

                        var outputParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        return outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<Category>> GetCategoriesByVendorAsync(int c_VendorId)
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetCategoriesByVendor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VendorId", c_VendorId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                            });
                        }
                    }
                }
            }
            return categories;
        }
    }
}
