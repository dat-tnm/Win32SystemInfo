using ConsoleApp1.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Management;
using System.Net;
using System.Text;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SysInfoConsole.Domain;
using SysInfoConsole.Services;
using System.Text.RegularExpressions;


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

    Console.WriteLine("Loading Data...");
    ISysInfoTable sysInfoTable = new SysInfoService();

    Console.WriteLine("Loading Done!");


    // Load an image
    using (var image = Image.Load<Rgba32>(initBgPath))
    {
        // Create a font
        string fontPath = Path.Combine(programDirectory, @"Roboto\Roboto-Light.ttf");

        var fontCollection = new FontCollection();
        fontCollection.Add(fontPath);
        var font = fontCollection.Get("Roboto Light");

        var primaryFont = font.CreateFont(24);
        var primaryColor = Color.Yellow;
        var secondaryColor = Color.White;
        var converter = new DataConverter();

        float x1 = 1200;
        float y = 800;
        float x2 = 1400;
        float deltaY = 30;


        // Add the text to the image
        image.Mutate(ctx => ctx.DrawText("Host Name", font.CreateFont(24), primaryColor, new PointF(x1, y) ));
        image.Mutate(ctx => ctx.DrawText(sysInfoTable.HostName, font.CreateFont(24), primaryColor, new PointF(x2, y) ));
        y += 20 + deltaY;

        image.Mutate(ctx => ctx.DrawText("IP Address", font.CreateFont(22), secondaryColor, new PointF(x1, y)));
        image.Mutate(ctx => ctx.DrawText(converter.ConvertToIPv4(sysInfoTable.IPAddress), font.CreateFont(22), secondaryColor, new PointF(x2, y)));
        y += deltaY;

        image.Mutate(ctx => ctx.DrawText("CPU", font.CreateFont(22), secondaryColor, new PointF(x1, y)));
        image.Mutate(ctx => ctx.DrawText(sysInfoTable.CPuName, font.CreateFont(22), secondaryColor, new PointF(x2, y)));
        y += deltaY;

        image.Mutate(ctx => ctx.DrawText("C:\\ Free Space", font.CreateFont(22), secondaryColor, new PointF(x1, y)));
        image.Mutate(ctx => ctx.DrawText(converter.ConvertBytesToGigabytes(sysInfoTable.FreeSpaceDiskC), font.CreateFont(22), secondaryColor, new PointF(x2, y)));
        y += deltaY;


        image.Save(sysInfoBgPath);
    }



    wallService.SetBackground(sysInfoBgPath);
}

class DataConverter()
{
    public string ConvertToIPv4(string ipAddress)
    {
        Match m = Regex.Match(ipAddress, @"\b\d+\.\d+\.\d+\.\d+\b");
        if (m.Success)
        {
            return m.Value;
        }

        return ipAddress;
    }

    public string ConvertBytesToGigabytes(string bytes)
    {
        if(double.TryParse(bytes, out var bytesVal))
        {
            bytesVal = Math.Round(bytesVal / 1073741824, 2);
            return bytesVal.ToString() + " GB";
        }

        return bytes + " GB";
    }
}