using System;
using NUnit.Framework;
using static TryHarder.TryHarder;

namespace TryHarder.Test
{
    [TestFixture]
    public class TryHarderTests
    {
        [Test]
        public void TryHarder1()
        {
            var cnt = 0;
            Try(5, () => { cnt++; }, null);
            Assert.AreEqual(1,cnt);
        }

        [Test]
        public void TryHarder5()
        {
            var cnt = 0;
            try
            {
                Try(5, () => { cnt++; throw new Exception();}, null);
            }
            catch (Exception)
            {
                //!
            }
            Assert.AreEqual(5, cnt);
        }

        [Test]
        public void Short()
        {
            var x = Try(() => 55);
            Assert.AreEqual(55,x);
        }

        [Test]
        public void Wrap1()
        {
            try
            {
                int WrapAction()
                {
                    throw new Exception();
                }
                var n = Wrap("meh", WrapAction);
                Console.WriteLine(n);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("meh"));
            }
        }
    }
}
