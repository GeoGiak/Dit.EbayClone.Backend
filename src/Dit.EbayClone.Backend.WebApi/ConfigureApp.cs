using Dit.EbayClone.Backend.WebApi.Endpoints;

namespace Dit.EbayClone.Backend.WebApi;

public static class ConfigureApp
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/");

        AuthEndpoints.MapEndpoints(group);
        UserEndpoints.MapEndpoints(group);
        AuctionEndpoints.MapEndpoints(group);
        ItemCategoryEndpoints.MapEndpoints(group);
        BiddingEndpoint.MapEndpoints(group);

        return app;
    }
}