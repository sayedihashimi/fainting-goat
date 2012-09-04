namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public interface IConfigHelper {
        string GetConfigValue(string key, bool isRequired=false);
    }

    public class ConfigHelper : IConfigHelper {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ConfigHelper));
        
        public string GetConfigValue(string key, bool isRequired=false) {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }

            string result = ConfigurationManager.AppSettings[key];

            if (isRequired && string.IsNullOrEmpty(result)) {
                string message = string.Format("Missing required configuration value [{0}]", key);
                log.Error(message);
                throw new ConfigurationErrorsException(message);
            }

            return result;
        }
    }
}
