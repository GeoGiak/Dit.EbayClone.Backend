using Dit.EbayClone.Backend.Application.Authentication;
using Dit.EbayClone.Backend.Core.WebModels.Users;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;

namespace Dit.EbayClone.Backend.WebApi.Endpoints;

public class AuthEndpoints
{
    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
    {
        var group = endpoints.MapGroup("auth");

        group.MapPost("login", Login);
        group.MapPost("register", Register);
        group.MapPost("refresh", RefreshToken);
        group.MapPost("logout", Logout);
        
        return group;
    }

    private static async Task<Results<Ok<TokenDto>, BadRequest<string>>> Login(
        LoginUserDto loginUserDto,
        IUserAuthenticationService authenticationService,
        CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginAsync(loginUserDto, cancellationToken);

        return result.Success ? 
            TypedResults.Ok(result.Data) : 
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<Results<Ok<User>, BadRequest<string>>> Register(
        CreateUserDto registerUserDto,
        IUserAuthenticationService authenticationService,
        CancellationToken cancellationToken)
    {
        var result = await authenticationService.RegisterAsync(registerUserDto, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok(result.Data) :
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<Results<Ok<TokenDto>, BadRequest<string>>> RefreshToken(
        RefreshTokenDto refreshToken,
        IUserAuthenticationService authenticationService,
        CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.RefreshTokenAsync(refreshToken.Token, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok(result.Data) :
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<Results<Ok, BadRequest<string>>> Logout(
        RefreshTokenDto refreshToken,
        IUserAuthenticationService authenticationService,
        CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.LogoutAsync(refreshToken.Token, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok() :
            TypedResults.BadRequest(result.Error);
    }
}