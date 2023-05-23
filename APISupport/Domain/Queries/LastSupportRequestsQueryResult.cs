namespace APISupport.Domain.Queries;

public class LastSupportRequestsQueryResult
{
    public int Id { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Email { get; set; }
    public string? Problem { get; set; }
}