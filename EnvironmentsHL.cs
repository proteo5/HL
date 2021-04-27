using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proteo5.HL
{
    public class EnvironmentsHL
    {
        public static EnvironmentData GetEnviroment(string environment)
        {
            //Get the file contents and create an object from Json.
            var filePath = $"{Environment.CurrentDirectory}\\Environments\\{environment}.json";
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file '{filePath}' was not found.");

            string jsonString = File.ReadAllText(filePath);
            EnvironmentData environmentData = JsonConvert.DeserializeObject<EnvironmentData>(jsonString);
            return environmentData;
        }

        private static string GetMSSQLConnString(EnvironmentData envControl) =>
            $"Data Source={envControl.host};Initial Catalog={envControl.database};User Id={envControl.user};Password={envControl.password}";
    }

    public class EnvironmentData
    {
        public string environment { get; set; }
        public string host { get; set; }
        public string database { get; set; }
        public string schema { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string engine { get; set; }
    }
}
