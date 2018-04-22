using System;
using System.IO.Ports;

namespace KMTronicRelay
{
    public interface IKmtronicU1CrbRelay
    {
        void Switch(SwitchNumber number, SwitchAction action);
    }

    public class KmtronicU1CrbRelay : IKmtronicU1CrbRelay
    {
        private readonly SerialPort _port;

        private KmtronicU1CrbRelay(SerialPort port)
        {
            _port = port;
        }

        public void Switch(SwitchNumber number, SwitchAction action)
        {
            var write = new byte[] {
                0xFF, /* whatever... */
                (byte)number,
                (byte)action
            };
            _port.Write(write, 0, write.Length);
        }

        public static IKmtronicU1CrbRelay Create(SerialPort port) => new KmtronicU1CrbRelay(port);

        /// <example>
        /// <code>
        /// KmtronicU1CrbRelay.Use("COM3", relay =&gt;
        /// {
        ///     relay.Switch(SwitchNumber.One, SwitchAction.On);
        ///     Thread.Sleep(500);
        ///     relay.Switch(SwitchNumber.One, SwitchAction.Off);
        ///     Thread.Sleep(500);
        /// });
        /// </code>
        /// </example>
        /// <param name="port">"COM3" or "/dev/ttyUSB0"</param>
        /// <param name="action"></param>
        public static void Use(string port, Action<IKmtronicU1CrbRelay> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            using (var sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
            {
                sp.Open();
                try
                {
                    var relay = new KmtronicU1CrbRelay(sp);
                    action(relay);
                }
                finally
                {
                    sp.Close();
                }
            }
        }
    }

    public enum SwitchNumber : byte
    {
        One = 0x01,
        Two = 0x02,
    }

    public enum SwitchAction : byte
    {
        Off = 0x00,
        On = 0x01,
    }
}
