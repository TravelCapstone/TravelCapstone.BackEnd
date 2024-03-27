namespace TravelCapstone.BackEnd.Common.DTO;

public class AppActionResult
{
    public object Result { get; set; } 

    public bool IsSuccess { get; set; } = true;
    public List<string?> Messages { get; set; } = new();
}

