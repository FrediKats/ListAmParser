namespace ListAmParser.PageParser.SearchPageItemFilters;

public class SizeSearchPageItemFilter : ISearchPageItemFilter
{
    private readonly int _size;

    public SizeSearchPageItemFilter(int size)
    {
        _size = size;
    }

    public string PrepareArgument()
    {
        return $"_a3_1={_size}";
    }
}