using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUtil.DemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var processId = MTUtil.ProcessIdUsingTcpPort(42008);
            var processPath = MTUtil.ProcessPathUsingTcpPort(42008);
            var pid2 = MTUtil.ProcessIdByExecutablePath(@"C:\Program Files (x86)\MetaTrader 4 IC Markets\terminal.exe");
        }
    }
}
