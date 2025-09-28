using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Application.Authentication;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    
    RefreshToken GenerateRefreshToken(User user);
}