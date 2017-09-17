using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace lightcontrols
{
    class Program
    {
        static SerialLineHandler serialHandler;
        static SerialPort COMport;
        static FileMonitor fileMonitor;

        static void Main(string[] args)
        {
            Init();
            PerformTests();
            fileMonitor = new FileMonitor(serialHandler);

            while (true)
            {
                string s = Console.ReadLine();
                if (serialHandler.Write(s) == false)
                    Init();
            }
        }
        static void Init()
        {
            // Init all instances as null
            if (fileMonitor != null)
                fileMonitor.Stop();
            serialHandler = null;
            COMport = null;            
            GC.Collect();

            serialHandler = new SerialLineHandler();
            do
            {
                COMport = serialHandler.ActiveComPort;
                if (COMport == null)
                {
                    Console.WriteLine("{0} Could not detect lightControl system!", System.DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
            } while (COMport == null);            
        }

        static void PerformTests()
        {
            List<LightControllerCommand> testCmd = new List<LightControllerCommand>
            {
                new LightControllerCommand(Command.getVersion),
                new LightControllerCommand(Command.allLightsOn),
                new LightControllerCommand(Command.getLights),
                new LightControllerCommand(Command.allLightsOff),
                new LightControllerCommand(Command.getLights)
            };

            foreach (LightControllerCommand cmd in testCmd)
            {                
                if (serialHandler.Write(cmd) == false)
                    Init();
                Thread.Sleep(1000);
            }
        }
    }    
}
