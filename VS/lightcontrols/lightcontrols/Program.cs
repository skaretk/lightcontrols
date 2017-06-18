using System;
using System.IO.Ports;
using System.Threading;

namespace lightcontrols
{
    class Program
    {        
        static void Main(string[] args)
        {
            SerialLineHandler serialHandler = new SerialLineHandler();
            SerialPort COMport;
            do
            {
                COMport = serialHandler.ActiveComPort;
                if (COMport == null)
                {
                    Console.WriteLine("{0} Could not detect lightControl system!", System.DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
            } while (COMport == null);

            FileMonitor fileMonitor = new FileMonitor(serialHandler);

            LightControllerCommand[] testCmd = new LightControllerCommand[]
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
            };

            string[] test = new string[] { "255", "1,1", "2,1","3,1", "4,1", "5,1", "6,1", "7,1", "8,1", "9,1", "10,1", "11,1", "12,1", "13,1", "14,1", "15,1", "16,1","255" };

            foreach (string cmd in test)
            {
                serialHandler.Write(cmd);
            }

            while (true)
            {
                string s = Console.ReadLine();
                serialHandler.Write(s);
            }
        }
    }
}
