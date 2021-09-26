using System;
using Microsoft.Extensions.Configuration;

namespace Compass.Security.Application.Commons.Constants
{
    public class SettingConstant
    {
        #region constant
        private const string Development = "Development";
        #endregion

        public static string ConnectionString { get; set; }

        public static void LoadSetting(IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment : {environment}");

            ConnectionString = environment is Development ? configuration.GetConnectionString("DbConnection") : Environment.GetEnvironmentVariable("DB_CONNECTION");

            Console.WriteLine($"ConnectionString : {ConnectionString}");
        }
    }
}