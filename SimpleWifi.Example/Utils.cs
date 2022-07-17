using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWifi.Example
{
    internal class Utils
    {

        /// <summary>
        /// 如果要切换成英文，可以在命令行下输入 chcp 437 变成英文：
        /// 如果想再切换回中文，只需要在命令行下输入 chcp 936 即可。
        /// </summary>
        /// <param name="strIp"></param>
        /// <returns></returns>
        public static PingResult CmdPing(string strIp)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            PingResult pingrst = PingResult.Other; ;
            p.Start();
            p.StandardInput.WriteLine("chcp 437 ");
            p.StandardInput.WriteLine("ping -n 1 " + strIp);
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            if (strRst.IndexOf("(0% loss)") != -1)
            {
                pingrst = PingResult.Connected;
            }
            else if (strRst.IndexOf("Destination host unreachable.") != -1)
            {
                pingrst = PingResult.Unreachable;
            }
            else if (strRst.IndexOf("Request timed out.") != -1)
            {
                pingrst = PingResult.TimeOut;
            }
            else if (strRst.IndexOf("Unknown host") != -1)
            {
                pingrst = PingResult.UnknownHost;
            }

            p.Close();
            return pingrst;
        }

    }

    public enum PingResult
    {
        Connected, Unreachable, TimeOut, UnknownHost, Other,
    }
}
