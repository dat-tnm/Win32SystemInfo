using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Win32Class
    {
        private readonly ManagementClass _osClass;
        private readonly List<PropertyData> _favoriteProperties = new List<PropertyData>();

        public List<PropertyData> Properties { get { return _favoriteProperties; } }

        public Win32Class(string win32class, string[] favoriteProperties)
        {
            // Get the WMI class
            _osClass = new(win32class);
            _osClass.Options.UseAmendedQualifiers = true;


            foreach (string propertyName in favoriteProperties)
            {
                try
                {
                    var property = _osClass.Properties[propertyName];
                    _favoriteProperties.Add(property);
                }
                catch { }
            }
        }


        public string GetPropertyValue(PropertyData property)
        {
            foreach (ManagementObject c in _osClass.GetInstances())
            {
                object val = c.Properties[property.Name.ToString()].Value;

                if (val == null)
                    continue;

                var res = val is string[] ? string.Join("; ", val as string[]) : val.ToString();

                if (!string.IsNullOrEmpty(res))
                    return res;
            }

            return string.Empty;
        }
    }
}
