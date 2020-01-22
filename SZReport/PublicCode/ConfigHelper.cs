using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.PublicCode
{
    public static class ConfigHelper
    {
        private static IConfiguration _configuration;

        private static IConfiguration _dataconfiguration;

        static ConfigHelper()
        {
            _configuration = BuildConfiguration("appsettings.json");

            _dataconfiguration = BuildConfiguration("MSSQL.json");
        }

        private static IConfiguration BuildConfiguration(string fileName)
        {

            IConfiguration conf;
            //在当前目录或者根目录中寻找appsettings.json文件


            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            conf = builder.Build();

            return conf;
        }

        public static string GetSectionValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
