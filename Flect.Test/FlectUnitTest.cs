using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flect.Test
{
    [TestClass]
    public class FlectUnitTest
    {
        [TestMethod]
        public void Reflection()
        {
            typeof(Dto).Flect();
            typeof(Moo).Flect();
            ObjectExtensions.Flect<Dto>();
            new Dto().Flect();
        }
    }

    public class Dto
    {
        public int I { get; set; }
        public string S { get; set; }
        public long L { get; set; }
        public object O { get; set; }
    }

    internal static class Moo
    {
        private static int I { get; set; }
        private static string S { get; set; }
        private static long L { get; set; }
        private static object O { get; set; }
        private static Dto D1;
        private static Dto D2 = new Dto();
    }
}
