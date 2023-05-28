namespace ListAmParser.PageParser.SearchPageItemFilters;

public interface ISearchPageItemFilter
{
    string PrepareArgument();
}

public class PhotoOnlySearchPageItemFilter : ISearchPageItemFilter
{
    public string PrepareArgument()
    {
        return "po=1";
    }
}