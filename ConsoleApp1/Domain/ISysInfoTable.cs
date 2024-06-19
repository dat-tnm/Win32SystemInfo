using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoConsole.Domain
{
    public interface ISysInfoTable
    {
        public string HostName { get; }
        public string IPAddress { get; }
        public string CPuName { get; }
        public string FreeSpaceDiskC { get; }
    }
}
