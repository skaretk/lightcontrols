using System;
using System.IO.Ports;

namespace lightcontrols
{
    class Program
    {
        /// <summary>
        /// Light enum, extra commands to arduino
        /// </summary>
        enum Cmd
        {
            getVersion = 254,
            getLights = 255
        }
        /// <summary>
        /// 
        /// </summary>
        enum Val
        {
            Off,
            On
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static SerialPort detectComPort()
        {
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports) {
                SerialPort sp = new SerialPort(port, 9600);
                try{
                    sp.Open();                    
                    sp.WriteLine("254");
                    sp.ReadTimeout = 2000;
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            string reply = sp.ReadLine();
                            if (reply.StartsWith("lightControls"))
                            {
                                Console.WriteLine(String.Format("{0} detected @ port {1}!", reply.Replace("\r",""), port));
                                return sp;
                            }
                            else
                            {
                                Console.WriteLine(reply);
                            };
                        }catch (TimeoutException) { }
                    }
                }catch(Exception) {
                    sp.Close();
                    continue; // Serialport busy...
                }
            }
            return null;
        }
        static void Main(string[] args)
        {
            SerialPort COMport =  detectComPort();
            if (COMport == null)
            {
                Console.WriteLine("ERROR, Could not detect lightControl system!");
                Console.ReadKey();
                return;
            }

            try
            {
                if (args.Length == 1)
                    COMport.WriteLine(String.Format("{0}\n", args[0].ToString()));
                else if (args.Length == 2)
                    COMport.WriteLine(String.Format("{0},{1}\n", args[0].ToString(), args[1].ToString()));
                else { }
                while (true)
                {
                    Console.WriteLine(COMport.ReadLine());
                }
            }catch (Exception ex) {
                if (ex is TimeoutException) { }
                else
                    Console.WriteLine(ex.ToString());
            };
            COMport.Close();
            Console.Read();
        }
    }
}
