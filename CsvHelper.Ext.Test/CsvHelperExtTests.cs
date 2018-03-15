using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NUnit.Framework;

namespace CsvHelper.Ext.Test
{
    [TestFixture]
    public class CsvHelperExtTests
    {
        [Test]
        public void ReadTest1()
        {
            var file = DateTime.Now.Ticks + "-ReadTest1.csv";
            const string content = "Col1;Col2;Col3\r\n" +
                                   "a;1;1.22\r\n" +
                                   "b;2;2.33\r\n";
            File.WriteAllText(file, content, ExcelCsv.GetExcelAtDeConfig().Encoding);

            var res = ExcelCsv.ReadCsv(file, row => new
            {
                c1 = row[0],
                c2 = row.GetField<int>("Col2"),
                c3 = decimal.Parse(row[2], CultureInfo.InvariantCulture),
            });

            foreach (var r in res)
            {
                Console.WriteLine($"{r.c1};{r.c2};{r.c3}");
            }

            

        }

        [Test]
        public void Bytes()
        {

            var line1 = new {x = 1, y = 2};
            var line2 = new {x = "a", y = "b"};

            var data = new List<object>{line1, line2};

            var bytes = data.GetCsvBytes();
            var str = ExcelCsv.GetExcelAtDeConfig().Encoding.GetString(bytes).RemoveControlChars();
            Console.WriteLine(str);
            Assert.AreEqual("x;y1;2a;b", str);
        }

    }
}
