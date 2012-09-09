namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public interface IConfig {
        string GetConfigValue(string key, bool isRequired=false);

        IList<string> GetList(string key, char delimiter = ';', bool isRequired = false);
    }

    public class Config : IConfig {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Config));
        
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

        public IList<string> GetList(string key,char delimiter=';', bool isRequired = false) {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (delimiter == null) { throw new ArgumentNullException("delimiter"); }

            IList<string> result = null;
            string configValue = this.GetConfigValue(key, isRequired);
            if (configValue != null) {
                string[] values = configValue.Split(delimiter);
                if (values != null && values.Length > 0) {
                    result = values.ToList();
                }
            }

            return result;
        }
    }
}
