using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dit.EbayClone.Backend.Core.Options;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dit.EbayClone.Backend.Application.Authentication;

public class JwtService(IOptions<JwtOptions> jwtOptions): IJwtService
{
    private JwtOptions _jwtOptions = jwtOptions.Value;
    
    
    public string GenerateAccessToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetTokenClaims(user);
        
        var token = new JwtSecurityTokenHandler()
            .WriteToken(GetTokenOptions(signingCredentials, claims));
        return token;
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        return new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            User = user,
        };
    }

    private List<Claim> GetTokenClaims(User user)
    {
        return
        [
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ];
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GetTokenOptions(
        SigningCredentials signingCredentials,
        List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpirationMinutes)),
            signingCredentials: signingCredentials);
    }
    
}