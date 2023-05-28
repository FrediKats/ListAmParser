using System.Text;
using ListAmParser.Abstractions.Models;
using ListAmParser.ExcelExport;
using ListAmParser.PageParser;
using ListAmParser.PageParser.HtmlParsers;
using ListAmParser.PageParser.SearchPageItemFilters;

namespace ListAmParser.ConsoleClient;
public class Program
{
    public static async Task Main()
    {
        const int pageForParse = 11;
        using var catalogParsingPipeline = new CatalogParsingPipeline();

        List<ItemDescription> itemsWithOneRoom = await catalogParsingPipeline.Process(GetItemsWithRoomLimitFilters(1), pageForParse);
        List<ItemDescription> itemsWithTwoRoom = await catalogParsingPipeline.Process(GetItemsWithRoomLimitFilters(2), pageForParse);

        List<ItemDescription> itemDescriptions = itemsWithOneRoom
            .Concat(itemsWithTwoRoom)
            .DistinctBy(i => i.ItemId)
            .ToList();

        var outputPath = "C:\\Users\\fredi\\OneDrive\\Shared\\report.xlsx";
        var excelEntityExporter = new ExcelEntityExporter<ItemDescription>(outputPath, new CatalogItemIExcelElementDecomposer());
        excelEntityExporter.Export("Export", itemDescriptions);
    }

    private static SearchPageFilterUrlBuilder GetItemsWithRoomLimitFilters(int roomCount)
    {
        SearchPageFilterUrlBuilder buildUrl = new SearchPageFilterUrlBuilder()
            .Add(new PhotoOnlySearchPageItemFilter())
            .Add(PaymentTypeSearchPageItemFilter.Monthly())
            .Add(RepairTypeSearchPageItemFilter.Designer())
            .Add(RegionSearchPageItemFilter.Kentron())
            .Add(new RoomCountSearchPageItemFilter(roomCount))
            .Add(new SizeSearchPageItemFilter(50));

        return buildUrl;
    }

    private static void TestMethods()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string itemPageHtml = File.ReadAllText("D:\\Coding\\Other\\ListAmParser\\Resources\\Test-item.txt");
        ItemDescription itemDescription = new HtmlItemPageParser().Parse(itemPageHtml);
        Console.WriteLine(itemDescription);
        Console.WriteLine();
        Console.WriteLine();

        string itemListHtml = File.ReadAllText("D:\\Coding\\Other\\ListAmParser\\Resources\\Test-item-list.txt");
        List<int> itemList = new HtmlSearchPageParser().ParseItemList(itemListHtml);
        Console.WriteLine("Items: " + string.Join(", ", itemList));

        string buildUrl = GetItemsWithRoomLimitFilters(2).BuildUrl();

        Console.WriteLine(buildUrl);
    }
}