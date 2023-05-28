namespace ListAmParser.PageParser.SearchPageItemFilters;

public class RoomCountSearchPageItemFilter : ISearchPageItemFilter
{
    private readonly int _count;

    public RoomCountSearchPageItemFilter(int count)
    {
        _count = count;
    }

    public string PrepareArgument()
    {
        return $"_a4={_count}";
    }
}