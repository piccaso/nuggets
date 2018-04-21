using System;
using System.IO.Ports;
using System.Threading;
using NUnit.Framework;

namespace KMTronicRelay.test
{
    [TestFixture]
    public class KmtronicU1CrbRelayTest
    {
        [Test]
        public void RelayTest()
        {
            try
            {
                KmtronicU1CrbRelay.Use("COM3", relay =>
                {
                    relay.Switch(SwitchNumber.One, SwitchAction.On);
                    Thread.Sleep(500);
                    relay.Switch(SwitchNumber.One, SwitchAction.Off);
                    Thread.Sleep(500);
                });
            }
            catch (System.IO.IOException e)
            {
                TestContext.WriteLine(e);
                Assert.Inconclusive("Com port not available");
            }
        }

        [Test]
        public void EnumerateComPorts()
        {
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                TestContext.WriteLine(port);
            }
        }
    }
}
