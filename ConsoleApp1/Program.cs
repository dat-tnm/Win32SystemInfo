using ConsoleApp1.Models;
using System.Management;
using System.Net;
using System.Text;


PrintOntoBackground();


static void PrintOntoBackground()
{
    string programDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
    string initBgPath = Path.Combine(programDirectory, "init_bg.jpg");
    string sysInfoBgPath = Path.Combine(programDirectory, "sysinfo_bg.jpg");


    var wallService = new WallpaperService();
    string currentBgPath = wallService.GetWallpaperPath();

    if (currentBgPath != sysInfoBgPath)
    {
        File.Copy(currentBgPath, initBgPath, true);
    }





    var dict0 = new Dictionary<string, string>();
    dict0.Add("Win32_NetworkAdapterConfiguration", "Network.");
    dict0.Add("Win32_OperatingSystem", "Host.");
    dict0.Add("Win32_Processor", "CPU.");
    dict0.Add("Win32_LogicalDisk", "Disk.");

    // Get the WMI class
    var dict = new Dictionary<string, string[]>();
    dict.Add("Win32_NetworkAdapterConfiguration", new string[] { "IPAddress" });
    dict.Add("Win32_OperatingSystem", new string[] { "CSName" });
    dict.Add("Win32_Processor", new string[] { "Name" });
    dict.Add("Win32_LogicalDisk", new string[] { "DeviceID", "FreeSpace" });

    StringBuilder sb1 = new StringBuilder();
    StringBuilder sb2 = new StringBuilder();
    foreach (var key in dict.Keys)
    {
        var win32class = new Win32Class(key, dict[key]);
        foreach (PropertyData property in win32class.Properties)
        {
            sb1.Append(dict0[key]);
            sb1.AppendLine(property.Name);

            sb2.Append(": ");
            sb2.AppendLine(win32class.GetPropertyValue(property));
        }
    }



    Console.WriteLine(sb1);
    Console.WriteLine(sb2);
    //Console.WriteLine(initBgPath);


    var adjustImage = new DrawImageService();
    adjustImage.DrawText(initBgPath, sysInfoBgPath, sb1.ToString(), new SixLabors.ImageSharp.PointF(1200, 100));
    adjustImage.DrawText(sysInfoBgPath, sysInfoBgPath, sb2.ToString(), new SixLabors.ImageSharp.PointF(1400, 100));

    wallService.SetBackground(sysInfoBgPath);
}