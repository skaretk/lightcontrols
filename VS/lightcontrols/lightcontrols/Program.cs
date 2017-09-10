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
                new LightControllerCommand(Command.getLights),
                new LightControllerCommand(Command.light1,Value.On),
                new LightControllerCommand(Command.light2,Value.On),
                new LightControllerCommand(Command.light3,Value.On),
                new LightControllerCommand(Command.light4,Value.On),
                new LightControllerCommand(Command.light5,Value.On),
                new LightControllerCommand(Command.light6,Value.On),
                new LightControllerCommand(Command.light7,Value.On),
                new LightControllerCommand(Command.light8,Value.On),
                new LightControllerCommand(Command.light9,Value.On),
                new LightControllerCommand(Command.light10,Value.On),
                new LightControllerCommand(Command.light11,Value.On),
                new LightControllerCommand(Command.light12,Value.On),
                new LightControllerCommand(Command.light13,Value.On),
                new LightControllerCommand(Command.light14,Value.On),
                new LightControllerCommand(Command.light15,Value.On),
                new LightControllerCommand(Command.light16,Value.On),
                new LightControllerCommand(Command.getLights)
            };

            List<string> testCommands = new List<string> { "255", "1,1", "2,1", "3,1", "4,1", "5,1", "6,1", "7,1", "8,1", "9,1", "10,1", "11,1", "12,1", "13,1", "14,1", "15,1", "16,1", "255" };
            foreach (LightControllerCommand cmd in testCmd)
            {
                if (serialHandler.Write(cmd) == false)
                    Init();
            }
        }
    }    
}
