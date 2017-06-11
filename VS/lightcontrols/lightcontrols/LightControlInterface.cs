using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightcontrols
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
    /// LightcontrolInterface
    /// Interface between file handling and transmitting data to the arduino lightcontrol
    /// </summary>
    interface LightControlInterface
    {
        /// <summary>
        /// Read interface function
        /// </summary>
        /// <param name="args"></param>
        void Read(string input);

        /// <summary>
        /// Write interface function
        /// </summary>
        /// <param name="args"></param>
        void Write(string output);
    }
}
