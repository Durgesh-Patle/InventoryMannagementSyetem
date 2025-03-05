using System.Data;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace InventoryMannagementSystem.Services
{
    public class UpdateStockServices : IStock
    {
        private readonly string _connectionString;

        public UpdateStockServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> UpdateStockAsync(CVViewModel stock)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_UpdateStock", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ViewProductId", stock.ViewProductId);
                        command.Parameters.AddWithValue("@ViewCategoryId", stock.ViewCategoryId);
                        command.Parameters.AddWithValue("@VendorId", stock.VendorModel.VendorId);
                        command.Parameters.AddWithValue("@Billing", stock.VendorModel.Billing);
                        command.Parameters.AddWithValue("@Quantity", stock.VendorModel.Quantity);

                        SqlParameter returnMessageParam = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar, 255);
                        returnMessageParam.Direction = ParameterDirection.Output;

                        command.Parameters.Add(returnMessageParam);

                        await command.ExecuteNonQueryAsync();

                        return returnMessageParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


    }
}
