using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class WallpaperService
    {
        public string GetWallpaperPath()
        {
            string wallpaperPath = "";
            try
            {
                // Read the wallpaper path from the registry
                byte[] pathBytes = (byte[])Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop").GetValue("TranscodedImageCache");
                wallpaperPath = Encoding.Unicode.GetString(pathBytes[24..]).TrimEnd('\0');
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving wallpaper path: {ex.Message}");
            }
            return wallpaperPath;
        }

        public void PrintResult()
        {
            string wallpaperPath = GetWallpaperPath();
            if (!string.IsNullOrEmpty(wallpaperPath))
            {
                Console.WriteLine($"Current wallpaper path: {wallpaperPath}");
            }
            else
            {
                Console.WriteLine("Unable to retrieve wallpaper path.");
            }
        }

        public void SetBackground(string imgPath)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"PicturePosition", "10");
            key.SetValue(@"TileWallpaper", "0");
            key.Close();

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            SystemParametersInfo(20, 0, imgPath, 1 | 2);
        }
    }
}
