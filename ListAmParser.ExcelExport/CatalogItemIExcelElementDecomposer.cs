using ClosedXML.Excel;
using ListAmParser.Abstractions;
using ListAmParser.Abstractions.Models;

namespace ListAmParser.ExcelExport;

public class CatalogItemIExcelElementDecomposer : IExcelElementDecomposer<ItemDescription>
{
    public void SetupColumn(IXLWorksheet worksheet)
    {
        worksheet.Column(2).Width = 60;
        worksheet.Column(3).Width = 30;
    }

    public void AddTitle(IXLRow row)
    {
        row.Cell(1).Value = "Id";
        row.Cell(2).Value = "Title";
        row.Cell(3).Value = "Address";
        row.Cell(4).Value = "Rooms";
        row.Cell(5).Value = "Price";
        row.Cell(6).Value = "DW";
        row.Cell(7).Value = "AuthorName";
        row.Cell(8).Value = "Link";
        row.Cell(8).Value = "UserLink";
    }

    public void Decompose(IXLRow row, ItemDescription value)
    {
        if (value.Price is null)
            throw new ListAmException($"Cannot use item {value.ItemId}. Item does not contains price");

        row.Cell(1).Value = value.ItemId;
        row.Cell(2).Value = FilterTitle(value.Title);
        row.Cell(3).Value = value.Address;
        row.Cell(4).Value = value.RoomCount;
        row.Cell(5).Value = value.Price.Dollar;
        row.Cell(6).Value = value.HasDishWasher;
        row.Cell(7).Value = value.AuthorName;
        row.Cell(8).Value = "https://www.list.am/ru/item/" + value.ItemId;
        row.Cell(8).SetHyperlink(new XLHyperlink($"https://www.list.am/ru/item/" + value.ItemId));
        row.Cell(9).Value = value.UserProfileLink;
    }

    private string FilterTitle(string title)
    {
        return title
            .Replace("2-комн. квартира", "квартира")
            .Replace("квартира в новостройке", "квартира")
            .Replace("квартира в центре", "квартира")
            .Replace("квартира", "")
            .Replace(", высокие потолки", "")
            .Replace("Просторная", "")
            .Replace("1-комн.", "")
            .Replace("дизайнерский ремонт", "")
            .Replace("на ул. ", "")
            .Replace("на пр. ", "")
            .Trim(',', ' ');
    }
}