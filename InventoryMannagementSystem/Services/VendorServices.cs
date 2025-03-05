using System.Data;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem.Services
{
    public class VendorService : IVendor
    {
        private readonly string _connectionString;

        public VendorService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Vendor>> GetAllVendorAsync()
        {
            var vendors = new List<Vendor>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_GetAllVendors", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vendors.Add(new Vendor
                                {
                                    VendorId = reader.GetInt32(reader.GetOrdinal("VendorId")),
                                    VendorName = reader.GetString(reader.GetOrdinal("VendorName")),
                                    DateOfPurchase = reader.GetDateTime(reader.GetOrdinal("DateOfPurchase")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    VendorAddress = reader.GetString(reader.GetOrdinal("VendorAddress")),
                                    VendorEmail = reader.GetString(reader.GetOrdinal("VendorEmail")),
                                    Billing = reader.GetDecimal(reader.GetOrdinal("Billing")),
                                    Categories = new Category
                                    {
                                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                    },
                                    Products = new Product
                                    {
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
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
                Console.WriteLine($"Error in GetAllVendorAsync: {ex.Message}");
            }

            return vendors;
        }

        public async Task<List<Vendor>> GetUpdatedStockAsync()
        {
            var vendorProducts = new List<Vendor>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_GetUpdatedStock", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vendorProducts.Add(new Vendor
                                {

                                    VendorProductId = reader.GetInt32(reader.GetOrdinal("VendorProductId")),

                                    VendorId = reader.GetInt32(reader.GetOrdinal("VendorId")),
                                    VendorName = reader.GetString(reader.GetOrdinal("VendorName")),
                                    VendorAddress = reader.GetString(reader.GetOrdinal("VendorAddress")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Billing = reader.GetDecimal(reader.GetOrdinal("Billing")),
                                    DateOfPurchase = reader.GetDateTime(reader.GetOrdinal("DateOfSale")),
                                    Categories = new Category
                                    {
                                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                    },
                                    Products = new Product
                                    {
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
                                    },
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUpdatedStockAsync: {ex.Message}");
            }

            return vendorProducts;
        }

        public async Task<string> InsertVendorAsync(Vendor vendor)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("InsertVendor", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
                        cmd.Parameters.AddWithValue("@DateOfPurchase", vendor.DateOfPurchase);
                        cmd.Parameters.AddWithValue("@Quantity", vendor.Quantity);
                        cmd.Parameters.AddWithValue("@VendorAddress", vendor.VendorAddress);
                        cmd.Parameters.AddWithValue("@VendorEmail", vendor.VendorEmail);
                        cmd.Parameters.AddWithValue("@ProductId", vendor.Products.ProductId);
                        cmd.Parameters.AddWithValue("@CategoryId", vendor.Categories.CategoryId);
                        cmd.Parameters.AddWithValue("@Billing", vendor.Billing);

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
                return $"Error in InsertVendor: {ex.Message}";
            }
        }

        public async Task<string> DeleteVendorAsync(int id)
        {
            string returnMessage = string.Empty;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteVendor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@VendorProductId", id);

                        SqlParameter outputParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        await command.ExecuteNonQueryAsync();

                        returnMessage = outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                returnMessage = $"Error deleting vendor: {ex.Message}";
            }

            return returnMessage;
        }

        public async Task<Vendor> GetVendorByIdAsync(int id)
        {
            Vendor vendor = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_GetVendorById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VendorProductId", id);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                vendor = new Vendor
                                {
                                    VendorId = reader.GetInt32(reader.GetOrdinal("VendorId")),
                                    VendorName = reader.GetString(reader.GetOrdinal("VendorName")),
                                    DateOfPurchase = reader.GetDateTime(reader.GetOrdinal("DateOfPurchase")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    VendorAddress = reader.GetString(reader.GetOrdinal("VendorAddress")),
                                    VendorEmail = reader.GetString(reader.GetOrdinal("VendorEmail")),
                                    Billing = reader.GetDecimal(reader.GetOrdinal("Billing")),
                                    Categories = new Category
                                    {
                                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                    },
                                    Products = new Product
                                    {
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        ProductPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return vendor;
        }
        public async Task<string> UpdateVendorByIdAsync(Vendor vendor)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateVendorProductById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VendorProductId", vendor.VendorProductId);

                        cmd.Parameters.AddWithValue("@VendorId", vendor.VendorId);
                        cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
                        cmd.Parameters.AddWithValue("@VendorEmail", vendor.VendorEmail);
                        cmd.Parameters.AddWithValue("@VendorAddress", vendor.VendorAddress);
                        cmd.Parameters.AddWithValue("@Quantity", vendor.Quantity);

                        cmd.Parameters.AddWithValue("@ProductId", vendor.Products.ProductId);
                        cmd.Parameters.AddWithValue("@CategoryId", vendor.Categories.CategoryId);

                        cmd.Parameters.AddWithValue("@Billing", vendor.Billing);
                        cmd.Parameters.AddWithValue("@DateOfSale", vendor.DateOfPurchase);

                        var outputParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        return outputParam.Value.ToString();
                    };
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}
