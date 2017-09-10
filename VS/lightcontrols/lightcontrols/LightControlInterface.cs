using System;

namespace lightcontrols
{
    /// <summary>
    /// Light enum, extra commands to arduino
    /// </summary>
    enum Command
    {
        light1 = 1,
        light2,
        light3,
        light4,
        light5,
        light6,
        light7,
        light8,
        light9,
        light10,
        light11,
        light12,
        light13,
        light14,
        light15,
        light16,
        getVersion = 254,
        getLights = 255
    }
    /// <summary>
    /// 
    /// </summary>
    enum Value
    {
        Off,
        On
    }

    class LightControllerCommand
    {
        public LightControllerCommand(Command cmd)
        {
            this.Cmd = cmd.ToString("D");
        }
        public LightControllerCommand(Command cmd, Value val)
        {
            this.Cmd = cmd.ToString("D");
            this.Val = val.ToString("D");
        }
        
        private string Cmd { get; set; }
        private string Val { get; set; } = String.Empty;

        public override string ToString()
        {
            if (Val == String.Empty)
                return String.Format("{0}", Cmd);
            return String.Format("{0},{1}", Cmd, Val);
        }
    }

    /// <summary>
    /// LightcontrolInterface
    /// Interface between file handling and transmitting data to the arduino lightcontrol
    /// </summary>
    interface LightControlInterface
    {
        /// <summary>
        /// Read interface function
        /// </summary>
        /// <param name="args"></param>
        bool Read(string input);

        /// <summary>
        /// Write interface function
        /// </summary>
        /// <param name="args"></param>
        bool Write(string output);
    }
}
