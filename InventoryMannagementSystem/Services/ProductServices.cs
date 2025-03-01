using System.Data;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace InventoryMannagementSystem.Services
{
    public class ProductServices : IProduct
    {
        private readonly string _connectionString;

        public ProductServices(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_GetAllProducts", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Product
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Categories = new Category
                                    {
                                        //CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                    },
                                    ProductBrand = reader.GetString(reader.GetOrdinal("ProductBrand")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                                    ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
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

            return products;
        }

        public async Task<string> InsertProductAsync(Product product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.Parameters.AddWithValue("@CategoryId", product.Categories.CategoryId);
                        cmd.Parameters.AddWithValue("@ProductBrand", product.ProductBrand);
                        cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);


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
                return $"Error in InsertEmployeeAsync: {ex.Message}";
            }

        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetProductById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                ProductBrand = reader.GetString(reader.GetOrdinal("ProductBrand")),
                                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                                ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice")),
                                Categories = new Category
                                {
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                },
                            };
                        }
                    }
                }
            }
            return product;
        }

        public async Task<string> DeleteProductAsync(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_DeleteProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductId", id);

                        var outputParam = new SqlParameter
                        {
                            ParameterName = "@ReturnMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 100,
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
                Console.WriteLine($"Error in DeleteProductAsync: {ex.Message}");
                return "Error occurred while deleting product";
            }
        }

        public async Task<string> UpdateProductAsync(Product product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateProductById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.Parameters.AddWithValue("@ProductBrand", product.ProductBrand);
                        cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
                        //cmd.Parameters.AddWithValue("@CategoryId", product.Categories?.CategoryId);
                        cmd.Parameters.AddWithValue("@CategoryId", product.Categories.CategoryId);

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

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_GetProductsByCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Product
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return products;
        }

    }
}
