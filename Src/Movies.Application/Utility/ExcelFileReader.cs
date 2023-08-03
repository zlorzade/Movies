using OfficeOpenXml;

namespace Movies.Application
{
    public static class ExcelFileReader
    {

        public static List<T> Read<T>(Stream stream)
        {
            List<T> result;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(stream);
            var sheet = pck.Workbook.Worksheets[0];
            result = sheet.Cells[$"{sheet.Dimension.Start.Address}:{sheet.Dimension.End.Address}"].ToCollection<T>(options => options.HeaderRow = 0);
            stream.Close();
            stream.Dispose();
            return result;
        }
    }
}
