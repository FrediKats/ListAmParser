namespace ListAmParser.PageParser.SearchPageItemFilters;

public class PaymentTypeSearchPageItemFilter : ISearchPageItemFilter
{
    private readonly int _type;

    public PaymentTypeSearchPageItemFilter(int type)
    {
        _type = type;
    }

    public static PaymentTypeSearchPageItemFilter Monthly() => new PaymentTypeSearchPageItemFilter(1);
    public static PaymentTypeSearchPageItemFilter Daily() => new PaymentTypeSearchPageItemFilter(2);
    public string PrepareArgument() => $"pfreq={_type}";
}