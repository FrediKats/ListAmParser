namespace ListAmParser.PageParser.SearchPageItemFilters;

public class RepairTypeSearchPageItemFilter : ISearchPageItemFilter
{
    private readonly int _type;

    public RepairTypeSearchPageItemFilter(int type)
    {
        _type = type;
    }


    public static RepairTypeSearchPageItemFilter Designer() => new RepairTypeSearchPageItemFilter(6);
    public string PrepareArgument()
    {
        return $"_a38={_type}";
    }
}