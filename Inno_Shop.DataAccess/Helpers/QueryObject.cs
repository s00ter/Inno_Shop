namespace Inno_Shop.DataAccess.Helpers;

public class QueryObject
{
    public string? Title { get; set; } = null;
    public float? From { get; set; } = null;
    public float? To { get; set; } = null;
    public bool? PriceAscending { get; set; } = null;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}