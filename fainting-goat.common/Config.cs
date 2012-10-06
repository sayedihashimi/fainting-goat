namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public interface IConfig {
        string GetConfigValue(string key);
        string GetConfigValue(string key, bool isRequired);
        string GetConfigValue(string key, bool isRequired, string defaultValue);

        IList<string> GetList(string key, char delimiter = ';', bool isRequired = false, IList<string> defaultValue = null);
    }

    public class Config : IConfig {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Config));

        public string GetConfigValue(string key) {
            return this.GetConfigValue(key, false, null);
        }

        public string GetConfigValue(string key, bool isRequired) {
            return this.GetConfigValue(key, isRequired, null);
        }

        public string GetConfigValue(string key, bool isRequired,string defaultValue) {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }

            string result = ConfigurationManager.AppSettings[key];

            if (result == null) { result = defaultValue; }

            if (isRequired && string.IsNullOrEmpty(result)) {
                string message = string.Format("Missing required configuration value [{0}]", key);
                log.Error(message);
                throw new ConfigurationErrorsException(message);
            }

            return result;
        }

        public IList<string> GetList(string key,char delimiter=';', bool isRequired = false,IList<string>defaultValue = null) {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }

            IList<string> result = null;
            string configValue = this.GetConfigValue(key, isRequired);
            if (configValue != null) {
                string[] values = configValue.Split(delimiter);
                if (values != null && values.Length > 0) {
                    result = values.ToList();
                }
            }

            if (result == null) { result = defaultValue; }

            return result;
        }

        public static MdContentProviderType GetContentProviderTypeFromConfig(IConfig config) {
            if (config == null) { throw new ArgumentNullException("config"); }

            var result = MdContentProviderType.FileSystem;
            // if there is a value for gitUrl then it's a git repo otherwise filesystem
            string cfgValue = config.GetConfigValue(CommonConsts.AppSettings.GitUri);
            if (cfgValue != null) {
                result = MdContentProviderType.Git;
            }

            return result;
        }
    }
}