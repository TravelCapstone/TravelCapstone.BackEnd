namespace TravelCapstone.BackEnd.Common.DTO;

public class PagedResult<T>
{
    public List<T>? Items { get; set; }
    public int TotalPages { get; set; }
}