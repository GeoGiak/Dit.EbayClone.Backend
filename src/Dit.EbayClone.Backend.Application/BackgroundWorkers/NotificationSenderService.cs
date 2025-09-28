using Dit.EbayClone.Backend.Application.Auctions;
using Dit.EbayClone.Backend.Application.Notifications;
using Dit.EbayClone.Backend.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dit.EbayClone.Backend.Application.BackgroundWorkers;

public class NotificationSenderService(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationSenderService> logger)
    : BackgroundService
{
    private readonly ILogger<NotificationSenderService> _logger = logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = scopeFactory.CreateScope();
                var auctionService = scope.ServiceProvider.GetService<IAuctionService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var notificationServices = scope.ServiceProvider.GetServices<INotificationService>();
                
                var inactiveAuctions = await auctionService.GetUninformedClosedAuctions(stoppingToken);

                foreach (var auction in inactiveAuctions)
                {
                    if (auction.WinnerId is null)
                    {
                        continue;
                    }
                    
                    foreach (var notificationService in notificationServices)
                    {
                        await notificationService.SendNotificationAsync(
                            auction.WinnerId ?? Guid.Empty,
                            "Bid Winner",
                            $"You won the auction {auction.AuctionId}",
                            stoppingToken);
                        logger.LogInformation($"Bid Winner: {auction.WinnerId} was sent a notification");
                    }

                    auction.IsNotified = true;
                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}