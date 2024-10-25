using Newtonsoft.Json;

namespace EffectiveMobileTest1
{
    class Config
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public int TimeRangeMinutes { get; set; }

        public static Config LoadConfig(string configFilePath)
        {
            string json = File.ReadAllText(configFilePath);
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
