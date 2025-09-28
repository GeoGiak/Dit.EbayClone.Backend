using Dit.EbayClone.Backend.Application.Auctions;
using Dit.EbayClone.Backend.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dit.EbayClone.Backend.Application.BackgroundWorkers;

public class AuctionClosingService(
    IServiceScopeFactory scopeFactory, 
    ILogger<AuctionClosingService> logger)
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<AuctionClosingService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
                
                await auctionService.CloseExpiredAuctionsAsync(stoppingToken);
                
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}