using BasicIC_SendEmail.Interfaces;
using System.Web.Configuration;

namespace BasicIC_SendEmail.Config
{
    public class ConfigManager : IConfigManager
    {
        public string Get(string nameConfig)
        {
            return WebConfigurationManager.AppSettings[nameConfig];
        }

        public static string StaticGet(string nameConfig)
        {
            return WebConfigurationManager.AppSettings[nameConfig];
        }
    }
}