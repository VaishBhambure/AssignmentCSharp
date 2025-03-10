using Microsoft.Extensions.Configuration;


namespace PolicyApp.Uitility
{
    static class DbConnection
    {
        static IConfiguration _iConfiguration;

        static DbConnection()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()//used to build configuration object from dataSource
                        .SetBasePath(Directory.GetCurrentDirectory())//settting the path to applications current directory
                        .AddJsonFile("appsettings.json");//load the configuration setting from this json file
            _iConfiguration = builder.Build();//compiling configuration into Iconfiguration
        }

        public static string GetConnectionString()
        {
            return _iConfiguration.GetConnectionString("LocalConnectionString");
        }
    }
}   
