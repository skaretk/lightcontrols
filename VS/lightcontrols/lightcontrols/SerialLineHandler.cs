using System;
using System.IO.Ports;

using System.Threading;

namespace lightcontrols
{
    class SerialLineHandler : LightControlInterface
    {
        const int BaudRate = 115200;
        public bool Read(string input)
        {
            Console.WriteLine(input);
            return true;
        }

        public bool Write(LightControllerCommand command)
        {
            Console.WriteLine(DateTime.Now + " Sent: {0}", command.ToString());
            try
            {
                ActiveComPort.Write(command.ToString());
                Thread.Sleep(250);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }          
        }
        public bool Write(string output)
        {
            Console.WriteLine(DateTime.Now + " Sent: {0}", output);
            try
            {
                ActiveComPort.Write(output+"\r");
                Thread.Sleep(250);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public SerialLineHandler()
        {
        }

        ~SerialLineHandler()
        {
            if (ActiveComPort != null)
                ActiveComPort.Close();
        }

        private SerialPort activeComport { set; get;}
        public SerialPort ActiveComPort
        {
            get
            {
                if (this.activeComport == null)
                    this.activeComport = detectComPort();
                return this.activeComport;
            }
            set
            {
                this.activeComport = value;
            }
        }

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
                SerialPort sp = new SerialPort(port, BaudRate);
                try
                {
                    sp.Open();
                    // Thread.Sleep(2000);
                    sp.Write("254,\r");
                    sp.ReadTimeout = 2000;
                    try
                    {
                        string reply = sp.ReadTo("\n");
                        if (reply.StartsWith("lightControls"))
                        {
                            Console.WriteLine(String.Format("{0} detected @ port {1}!", reply.Replace("\r", ""), port));
                            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            return sp;
                        }
                        else
                        {
                            Console.WriteLine(reply);
                            sp.Close();
                        };
                    }
                    catch (TimeoutException)
                    {
                        Console.WriteLine(String.Format("Timeout {0}", port));
                        sp.Close();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(String.Format("{0} busy...", port));
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
            Console.Write(indata);
        }
    }
}
