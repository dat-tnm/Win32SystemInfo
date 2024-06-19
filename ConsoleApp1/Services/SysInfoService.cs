using ConsoleApp1.Models;
using SysInfoConsole.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoConsole.Services
{
    public class SysInfoService : ISysInfoTable
    {
        public Dictionary<string, SysInfoItemGroup> SysInfoGroups { get; }

        public string HostName => SysInfoGroups["Win32_OperatingSystem"].Items["CSName"].Value;

        public string IPAddress => SysInfoGroups["Win32_NetworkAdapterConfiguration"].Items["IPAddress"].Value;

        public string CPuName => SysInfoGroups["Win32_Processor"].Items["Name"].Value;

        public string FreeSpaceDiskC => SysInfoGroups["Win32_LogicalDisk"].Items["FreeSpace"].Value;

        public SysInfoService()
        {
            SysInfoGroups = new Dictionary<string, SysInfoItemGroup>();
            AddSysInfoItem("Win32_OperatingSystem", "CSName");
            AddSysInfoItem("Win32_NetworkAdapterConfiguration", "IPAddress");
            AddSysInfoItem("Win32_Processor", "Name");
            AddSysInfoItem("Win32_LogicalDisk", "FreeSpace");

            foreach (var sysInfoGroup in SysInfoGroups.Values)
            {
                string[] wmiProperties = sysInfoGroup.Items.Select(item => item.Value.WMIPropertyName).ToArray();

                var win32class = new Win32Class(sysInfoGroup.WMIName, wmiProperties);
                foreach (PropertyData property in win32class.Properties)
                {
                    sysInfoGroup.Items[property.Name].Value = win32class.GetPropertyValue(property);
                }
            }
        }

        public void AddSysInfoItem(string win32WMI, string wmiPropertyName)
        {
            if (!SysInfoGroups.ContainsKey(win32WMI))
            {
                SysInfoGroups.Add(win32WMI, new SysInfoItemGroup(win32WMI));
            }

            SysInfoGroups[win32WMI].Items.Add(wmiPropertyName, new SysInfoItem(wmiPropertyName));
        }
    }

    public class SysInfoItem
    {
        public SysInfoItem(string wmiPropertyName)
        {
            WMIPropertyName = wmiPropertyName;
        }

        public string WMIPropertyName { get; set; }
        public string Value { get; set; }
    }

    public class SysInfoItemGroup
    {
        public string WMIName { get; set; }
        public Dictionary<string, SysInfoItem> Items { get; }

        public SysInfoItemGroup(string wmiName)
        {
            WMIName = wmiName;
            Items = new Dictionary<string, SysInfoItem>();
        }
    }
}
