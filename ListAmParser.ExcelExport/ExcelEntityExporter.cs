using ClosedXML.Excel;

namespace ListAmParser.ExcelExport;

public class ExcelEntityExporter<T>
{
    private readonly string _worksheetName;
    private readonly IExcelElementDecomposer<T> _elementDecomposer;

    public ExcelEntityExporter(
        string worksheetName,
        IExcelElementDecomposer<T> elementDecomposer
        )
    {
        _worksheetName = worksheetName;
        _elementDecomposer = elementDecomposer;
    }

    public void Export(string page, IReadOnlyList<T> values)
    {
        using var workbook = new XLWorkbook(_worksheetName);

        if (workbook.Worksheets.Contains(page))
            workbook.Worksheets.Delete(page);

        IXLWorksheet worksheet = workbook.Worksheets.Add(page);

        _elementDecomposer.SetupColumn(worksheet);
        _elementDecomposer.AddTitle(worksheet.Row(1));
        for (var i = 0; i < values.Count; i++)
            _elementDecomposer.Decompose(worksheet.Row(i + 2), values[i]);

        workbook.SaveAs(_worksheetName);
    }
}