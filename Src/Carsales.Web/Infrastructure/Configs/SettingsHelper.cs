using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Carsales.Web.Infrastructure.Configs
{
    public static class SettingsHelper
    {
        public static T GetSettings<T>(string sectionName) where T : new()
        {
            var configSection = ConfigurationManager.GetSection(sectionName) as NameValueCollection;

            if (configSection == null) throw new ConfigurationErrorsException($"Failed to load PlainObjectConfigSection from section name {sectionName}");

            var keys = configSection.AllKeys;

            var result = new T();

            var properties = result.GetType().GetProperties().Where(x => x.CanWrite);

            foreach (var propertyInfo in properties)
            {
                var propertyName = propertyInfo.Name;
                if(keys.Any(x => string.Equals(x, propertyName, StringComparison.OrdinalIgnoreCase)))
                {
                    var value = configSection[propertyName];
                    if (value != null)
                    {
                        // need to test
                        //propertyInfo.SetValue(result, Convert.ChangeType(value, propertyInfo.PropertyType));
                        propertyInfo.SetValue(result, value);
                    }
                }
            }

            return result;
        }
    }
}