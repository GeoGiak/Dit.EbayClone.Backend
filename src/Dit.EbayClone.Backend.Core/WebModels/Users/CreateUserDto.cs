using Dit.EbayClone.Backend.Core.WebModels.Locations;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Core.WebModels.Users;

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public UserRoles Role { get; set; }
    
    public LocationDto LocationDto { get; set; }
}