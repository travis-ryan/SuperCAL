﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SuperCAL
{
    class Misc
    {
        public static void RestartWindows()
        {
            Logger.Log("Restarting windows...");
            Process.Start("shutdown.exe", "/r /t 0");
        }
        public static Task RunPowershell(string Command)
        {
            return Task.Run(() => {
                Process ps = new Process();
                ps.StartInfo.FileName = "powershell.exe";
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ps.StartInfo.Arguments = "-Command " + Command;
                ps.Start();
                ps.WaitForExit();
            });
        }
        public static Task RunCMD(string Command)
        {
            return Task.Run(() => {
                Process ps = new Process();
                ps.StartInfo.FileName = "cmd.exe";
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ps.StartInfo.Arguments = "/C " + Command;
                ps.Start();
                ps.WaitForExit();
            });
        }
        public static Task InstallPhaseTwo(bool install = true)
        {
            return Task.Run(() => {
                if (install)
                {
                    Directory.CreateDirectory(@"C:\MICROS\SuperCAL");
                    Logger.Good(@"C:\MICROS\SuperCAL: Created.");
                    File.WriteAllBytes(@"C:\MICROS\SuperCAL\PsExec.exe", Properties.Resources.PsExec);
                    Logger.Good(@"C:\MICROS\SuperCAL\PsExec.exe: Copied.");
                    File.WriteAllBytes(@"C:\MICROS\SuperCAL\SuperCALTask.xml", Properties.Resources.SuperCALTask);
                    Logger.Good(@"C:\MICROS\SuperCAL\SuperCALTask.xml: Copied.");
                    File.Copy("SuperCAL.xml", @"C:\MICROS\SuperCAL\SuperCAL.xml", true);
                    Logger.Good(@"C:\MICROS\SuperCAL\SuperCAL.xml: Copied.");
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, @"C:\MICROS\SuperCAL\SuperCAL.exe", true);
                    Logger.Good(@"C:\MICROS\SuperCAL\SuperCAL.exe: Copied.");
                    Logger.Log("Creating SuperCAL task...");
                    RunCMD(@"schtasks.exe /Create /tn SuperCAL /XML C:\MICROS\SuperCAL\SuperCALTask.xml");
                    Logger.Good("Done.");
                }
                else
                {
                    Logger.Log("Removing SuperCAL task...");
                    RunCMD("schtasks.exe /Delete /f /tn SuperCAL");
                    Logger.Good("Done.");
                }
            });
        }
        public static Task InstallNetdom()
        {
            return Task.Run(() => {
                File.WriteAllBytes(@"C:\Windows\System32\netdom.exe", Properties.Resources.netdom);
                Logger.Good(@"C:\Windows\System32\netdom.exe: Copied.");
                File.WriteAllBytes(@"C:\Windows\System32\en-US\netdom.exe.mui", Properties.Resources.netdom_mui);
                Logger.Good(@"C:\Windows\System32\en-US\netdom.exe.mui: Copied.");
            });
        }
    }
}
