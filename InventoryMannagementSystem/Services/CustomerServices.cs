using System.Data;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace InventoryMannagementSystem.Services
{
    public class CustomerServices : ICustomer
    {
        private readonly string _connectionString;

        public CustomerServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Customer>> GetAllCustomerAsync()
        {
            var customers = new List<Customer>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_GetAllCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customers.Add(new Customer
                                {
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerAddress = reader.GetString(reader.GetOrdinal("CustomerAddress")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    DateOfPurchase = reader.GetDateTime(reader.GetOrdinal("DateOfPurchase")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    TotalBillingAmount = reader.GetDecimal(reader.GetOrdinal("TotalBillingAmount")),
                                    Categories = new Category
                                    {
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                    },
                                    Products = new Product
                                    {
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllCustomersAsync: {ex.Message}");
            }

            return customers;
        }

        public async Task<string> InsertCustomerAsync(Customer customer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerAddress", customer.CustomerAddress);
                        cmd.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                        cmd.Parameters.AddWithValue("@DateOfPurchase", customer.DateOfPurchase);
                        cmd.Parameters.AddWithValue("@Quantity", customer.Quantity);
                        cmd.Parameters.AddWithValue("@TotalBillingAmount", customer.TotalBillingAmount);
                        cmd.Parameters.AddWithValue("@ProductId", customer.Products.ProductId);
                        cmd.Parameters.AddWithValue("@CategoryId", customer.Categories.CategoryId);

                        var outputParam = new SqlParameter
                        {
                            ParameterName = "@ReturnMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 255,
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
                return $"Error in InsertCustomer: {ex.Message}";
            }
        }


        public async Task<string> DeleteCustomerAsync(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_DeleteCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", id);

                        SqlParameter outputParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 255)
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
                return $"Error deleting Customer: {ex.Message}";
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            Customer customer = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_GetCustomerById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", id);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                customer = new Customer
                                {
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    CustomerAddress = reader.GetString(reader.GetOrdinal("CustomerAddress")),
                                    DateOfPurchase = reader.GetDateTime(reader.GetOrdinal("DateOfPurchase")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    TotalBillingAmount = reader.GetDecimal(reader.GetOrdinal("TotalBillingAmount")),

                                    Categories = new Category
                                    {
                                        CategoryId=reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    },
                                    Products = new Product
                                    {
                                        ProductId=reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice")),
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer details: {ex.Message}");
            }

            return customer;
        }

        public async Task<string> UpdateCustomerByIdAsync(Customer customer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                        cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerAddress", customer.CustomerAddress);
                        cmd.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                        cmd.Parameters.AddWithValue("@DateOfPurchase", customer.DateOfPurchase);
                        cmd.Parameters.AddWithValue("@Quantity", customer.Quantity);
                        cmd.Parameters.AddWithValue("@TotalBillingAmount", customer.TotalBillingAmount);
                        cmd.Parameters.AddWithValue("@ProductId", customer.Products.ProductId);
                        cmd.Parameters.AddWithValue("@CategoryId", customer.Categories.CategoryId);

                        SqlParameter returnMsgParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 255);
                        returnMsgParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(returnMsgParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        return returnMsgParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex.Message}";

            }
        }
    }
}
