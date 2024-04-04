namespace TravelCapstone.BackEnd.Common
{
    public class ExcelValidatingResponse
    {
        public bool IsValidated { get; set; }
        public string? HeaderError { get; set; }
        public string[]? Errors { get; set; }
    }
}