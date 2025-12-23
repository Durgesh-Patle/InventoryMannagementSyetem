namespace InventoryMannagementSystem.Common
{
    public static class Helper
    {
        private static IConfiguration _configuration;
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Initialize(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public const string CONNECTION_STRING_NAME = "DefaultConnection";

        #region Ye method configuration-based path me error logs ko timestamp ke sath text file me append karta hai
        public static void WriteLog(string message)
        {
            string logDir = _configuration["ErrorLogFile"];
            if (string.IsNullOrEmpty(logDir))
                logDir = Path.Combine(AppContext.BaseDirectory, "ErrorLogFiles");

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            string logFile = Path.Combine(logDir, "Inventory_Error_Log.txt");

            using StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine($"{DateTime.Now:dd-MMM-yy HH:mm:ss}\t{message}");
        }
        #endregion  
    }
}
