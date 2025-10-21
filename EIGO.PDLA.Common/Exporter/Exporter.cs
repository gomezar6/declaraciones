using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace EIGO.PDLA.Common.Exporter
{
    public static class Exporter
    {
        public static Stream ExportToCSV<T>(IEnumerable<T> elements,bool encabezado)
        {
            Stream stream = new MemoryStream();
            using var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", Encoding = Encoding.UTF8,HasHeaderRecord=encabezado };
            using var csv = new CsvWriter(writer, config);





            csv.WriteRecords(elements);
            csv.Flush();
            return stream;
        }


    }
}
