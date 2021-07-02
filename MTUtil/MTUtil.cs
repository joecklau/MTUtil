using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Net.NetworkInformation;
//using Vanara.PInvoke;

namespace MTUtil
{
    public class MTUtil
    {
        [DllExport]
        public static int FreeTcpPort(int start = 40000, int max = 65535, bool randomize = true)
        {
            if (randomize)
            {
                start += new Random(DateTime.Now.Millisecond).Next(0, (max - start) / 2);
            }

            TcpListener l = new TcpListener(IPAddress.Loopback, start);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        [DllExport]
        public static int ProcessIdUsingTcpPort(int port)
        {
            var details = new TcpHelperUtil().GetPortDetails(port);
            return details.Item1 ? details.Item2.ProcessID : 0;

            //// p.s. Vanara's version / IpHlpApi's version doesn't work on just Listening but not established connection
            ////var tcp6Connections = IpHlpApi.GetExtendedTcpTable<IpHlpApi.MIB_TCPTABLE_OWNER_PID>(IpHlpApi.TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
            ////var target6Connections = tcp6Connections.Where(x => x.dwState == IpHlpApi.MIB_TCP_STATE.MIB_TCP_STATE_LISTEN).ToArray();
            ////var tcpConnections = IpHlpApi.GetTcpTable2();
            ////var targetConnections = tcpConnections.Where(x => x.dwLocalPort == port).ToArray();
            ////return targetConnections.Any() ? targetConnections.First().dwOwningPid : 0;
        }

        [DllExport]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string ProcessPathUsingTcpPort(int port)
        {
            var details = new TcpHelperUtil().GetPortDetails(port);
            return details.Item1 ? details.Item2.Path : null;
        }

        [DllExport]
        public static int ProcessIdByExecutablePath([MarshalAs(UnmanagedType.LPWStr)] string processPath)
        {
            if (string.IsNullOrWhiteSpace(processPath)) return 0;

            var executableFileInfo = new FileInfo(processPath);
            if (!executableFileInfo.Exists) return 0;

            var processName = executableFileInfo.Name.Replace(executableFileInfo.Extension, string.Empty);
            var process = Process.GetProcessesByName(processName).FirstOrDefault(x => processPath.Equals(x.MainModule.FileName, StringComparison.InvariantCultureIgnoreCase));
            return process?.Id ?? 0;
        }

        [DllExport]
        public static bool IsMtApiPortDuplicatedWithOthers([MarshalAs(UnmanagedType.LPWStr)] string mtApiPortFilesDir, [MarshalAs(UnmanagedType.LPWStr)] string mtApiPortFileToSkip, int mtApiPort)
        {
            if (string.IsNullOrWhiteSpace(mtApiPortFilesDir)) return false;

            var mtApiPortFilesDirInfo = new DirectoryInfo(mtApiPortFilesDir);
            if (!mtApiPortFilesDirInfo.Exists) return false;

            var otherFiles = mtApiPortFilesDirInfo.GetFiles("*MtApiPort.txt").Where(x => !x.Name.Equals(mtApiPortFileToSkip, StringComparison.InvariantCultureIgnoreCase)).ToList();
            foreach(var otherFile in otherFiles)
            {
                var otherText = File.ReadLines(otherFile.FullName).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(otherText) && int.TryParse(otherText, out int parsedPort) && parsedPort == mtApiPort)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
