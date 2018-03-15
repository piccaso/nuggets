using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvConfig = CsvHelper.Configuration.Configuration;
using IEnum = System.Collections.IEnumerable;

namespace CsvHelper.Ext
{
    public static class ExcelCsv
    {
        public static CsvConfig GetExcelAtDeConfig() => new CsvConfig
        {
            Delimiter = ";",
            Encoding = GetAnsiEncoding(),
        };

        public static Encoding GetAnsiEncoding()
        {
#if NETSTD20
            return CodePagesEncodingProvider.Instance.GetEncoding(1252);
#endif
#if NET45
            return Encoding.GetEncoding(1252);
#endif
        }

        public static IList<T> ReadCsv<T>(string fileName, CsvConfig config = null)
        {
            config = config ?? GetExcelAtDeConfig();
            using (var fs = File.OpenRead(fileName))
            using (var sr = new StreamReader(fs, config.Encoding))
            {
                var csvReader = new CsvReader(sr, config);
                return csvReader.GetRecords<T>().ToList();
            }
        }

        public static IList<T> ReadCsv<T>(string fileName, Func<IReaderRow, T> readFunc) => ReadCsv(null, fileName, readFunc);
        public static IList<T> ReadCsv<T>(this CsvConfig config, string fileName, Func<IReaderRow,T> readFunc)
        {
            config = config ?? GetExcelAtDeConfig();
            using (var fs = File.OpenRead(fileName))
            using (var sr = new StreamReader(fs, config.Encoding))
            {
                var records = new List<T>();
                var csvReader = new CsvReader(sr, config);
                if (config.HasHeaderRecord)
                {
                    csvReader.Read();
                    csvReader.ReadHeader();
                }

                while (csvReader.Read())
                {
                    records.Add(readFunc(csvReader));
                }

                return records;
            }
        }

        public static void WriteCsvTo(this CsvConfig config, IEnum data, Stream stream) => WriteCsvTo(data, stream, config);
        public static void WriteCsvTo(this IEnum data, Stream stream, CsvConfig config = null)
        {
            config = config ?? GetExcelAtDeConfig();
            using (var sw = new StreamWriter(stream, config.Encoding))
            {
                var csvWriter = new CsvWriter(sw, config);
                csvWriter.WriteRecords(data);
            }
        }
        public static void WriteCsv(this CsvConfig config, IEnum data, string filename) => WriteCsv(data, filename, config);
        public static void WriteCsv(this IEnum data, string filename, CsvConfig config = null)
        {
            using (var fs = File.OpenWrite(filename))
            {
                data.WriteCsvTo(fs, config);
            }
        }
        public static byte[] GetCsvBytes(this IEnum data, CsvConfig config = null)
        {
            using (var ms = new MemoryStream())
            {
                data.WriteCsvTo(ms, config);
                return ms.ToArray();
            }
        }

        public static string RemoveControlChars(this string str) => new string(str.Where(c => !char.IsControl(c)).ToArray());
    }
}
