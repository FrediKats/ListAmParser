using ClosedXML.Excel;

namespace ListAmParser.ExcelExport;

public interface IExcelElementDecomposer<T>
{
    void SetupColumn(IXLWorksheet worksheet);
    void AddTitle(IXLRow row);
    void Decompose(IXLRow row, T value);
}