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
            SerialPort COMport = serialHandler.ActiveComPort;

            if (COMport == null)
            {
                Console.WriteLine("ERROR, Could not detect lightControl system!");
                Console.ReadKey();
                return;
            }

            FileMonitor fileMonitor = new FileMonitor(serialHandler);

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
