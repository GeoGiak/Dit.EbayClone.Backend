
using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Users;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Application.Authentication;

public class UserAuthenticationService(
    IJwtService jwtService,
    ApplicationDbContext dbContext): IUserAuthenticationService
{
    private readonly IJwtService jwtService = jwtService;
    private readonly ApplicationDbContext dbContext = dbContext;
    
    public async Task<Result<TokenDto>> LoginAsync(LoginUserDto loginUser, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(loginUser.Username), cancellationToken: cancellationToken);
        if (user == null || !CheckPassword(loginUser.Password, user.Password))
        {
            return Result<TokenDto>.Fail("Invalid username or password");
        }

        if (user.Status != UserStatus.Approved)
        {
            return Result<TokenDto>.Fail("User not approved");
        }
        
        if (!user.IsActive)
        {
            return Result<TokenDto>.Fail("User is not active");
        }

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshToken = jwtService.GenerateRefreshToken(user);
        
        user.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var tokens = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
        
        return Result<TokenDto>.Ok(tokens);
    }

    public async Task<Result<User>> RegisterAsync(CreateUserDto newUser, CancellationToken cancellationToken = default)
    {
        if (!CheckRole(newUser.Role))
        {
            Result<User>.Fail("Invalid role");
        }

        var user = new User
        {
            Username = newUser.UserName,
            Password = HashPassword(newUser.Password),
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            MobileNumber = newUser.MobileNumber,
            IsActive = true,
            Role = newUser.Role,
            Status = UserStatus.Pending
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        var res = Result<User>.Ok(user);
        return res;
    }

    public async Task<Result<string>> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken: cancellationToken);

        if (token is null)
        {
            return Result<string>.Fail("Invalid refresh token");
        }
        
        token.IsRevoked = true;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<string>.Ok("Success");
    }

    public async Task<Result<TokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var oldRefreshToken = await dbContext.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token.Equals(refreshToken), cancellationToken: cancellationToken);

        if (oldRefreshToken == null || oldRefreshToken.IsRevoked || oldRefreshToken.Expires < DateTime.UtcNow)
        {
            return Result<TokenDto>.Fail("Invalid refresh token");
        }
        
        var newTokens = new TokenDto
        {
            AccessToken = jwtService.GenerateAccessToken(oldRefreshToken.User),
            RefreshToken = oldRefreshToken.Token
        };
        
        return Result<TokenDto>.Ok(newTokens);
    }

    private bool CheckRole(UserRoles role)
    {
        return role == UserRoles.Bidder || role == UserRoles.Seller;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool CheckPassword(string toCheck, string originalHashed)
    {
        return BCrypt.Net.BCrypt.Verify(toCheck, originalHashed);
    }
}