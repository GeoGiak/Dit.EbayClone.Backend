using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Users;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Application.Authentication;

public interface IUserAuthenticationService
{
    Task<Result<TokenDto>> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);

    Task<Result<User>> RegisterAsync(CreateUserDto newUser, CancellationToken cancellationToken);

    Task<Result<string>> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<Result<TokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}