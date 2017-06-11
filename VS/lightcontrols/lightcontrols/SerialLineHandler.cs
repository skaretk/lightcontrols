using System;
using System.IO.Ports;

using System.Threading;

namespace lightcontrols
{
    class SerialLineHandler : LightControlInterface
    {
        public void Read(string input)
        {
            Console.WriteLine(input);
        }

        public void Write(string output)
        {
            Console.WriteLine("Tx: {0}", output);
            ActiveComPort.Write(output);
            Thread.Sleep(1500);
        }

        public SerialLineHandler()
        {
            ActiveComPort = detectComPort();
            if (ActiveComPort != null)
            {
                ActiveComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
        }

        ~SerialLineHandler()
        {
            if (ActiveComPort != null)
                ActiveComPort.Close();
        }

        public SerialPort ActiveComPort { get; set; }

        /// <summary>
        /// Will search for the arduino over serial line
        /// </summary>
        /// <returns></returns>
        private SerialPort detectComPort()
        {
            // Get list of serial port names.
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Console.WriteLine(String.Format("Searching.. {0}", port));
                // Open serialport and try to request data to detect the lightcontrol system
                SerialPort sp = new SerialPort(port, 9600);
                try
                {
                    sp.Open();
                    sp.Write("254");
                    sp.ReadTimeout = 2000;
                   /* for (int i = 0; i < 5; i++)
                    {*/
                        try
                        {
                            string reply = sp.ReadLine();
                            if (reply.StartsWith("lightControls"))
                            {
                                Console.WriteLine(String.Format("{0} detected @ port {1}!", reply.Replace("\r", ""), port));
                                return sp;
                            }
                            else
                            {
                                Console.WriteLine(reply);
                            };
                        }
                        catch (TimeoutException) { }
                   // }
                }
                catch (Exception)
                {
                    sp.Close();
                    continue; // Serialport busy...
                }
            }
            return null;
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine("Data Received:");
            Console.Write(indata);
        }

    }
}
