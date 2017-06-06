using System;
using System.IO.Ports;

namespace lightcontrols
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            SerialPort sp = new SerialPort(ports[0],9600);
            sp.Open();
            try
            {
                sp.WriteLine(args[0].ToString());
            }catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            };
            sp.Close();
        }
    }
}
