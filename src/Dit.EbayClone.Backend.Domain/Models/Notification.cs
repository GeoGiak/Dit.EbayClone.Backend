namespace Dit.EbayClone.Backend.Domain.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}