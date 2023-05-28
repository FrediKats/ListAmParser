namespace ListAmParser.PageParser.SearchPageItemFilters;

public class RegionSearchPageItemFilter : ISearchPageItemFilter
{
    private readonly int _type;

    public RegionSearchPageItemFilter(int type)
    {
        _type = type;
    }

    public static RegionSearchPageItemFilter Kentron() => new RegionSearchPageItemFilter(8);
    public string PrepareArgument() => $"n={_type}";
}