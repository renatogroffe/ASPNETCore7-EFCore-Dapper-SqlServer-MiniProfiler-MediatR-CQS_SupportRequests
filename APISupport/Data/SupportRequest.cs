namespace APISupport.Data;

public class SupportRequest
{
    public int Id { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Email { get; set; }
    public string? Problem { get; set; }
}