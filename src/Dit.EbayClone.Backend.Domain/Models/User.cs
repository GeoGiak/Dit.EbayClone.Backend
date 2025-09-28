namespace Dit.EbayClone.Backend.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = false;
    
    public UserRoles Role { get; set; } = UserRoles.Visitor;

    public UserStatus Status { get; set; } = UserStatus.Pending;
    
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}