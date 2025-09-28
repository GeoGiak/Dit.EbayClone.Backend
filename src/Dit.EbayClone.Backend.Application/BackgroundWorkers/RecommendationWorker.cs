using Dit.EbayClone.Backend.Application.Recommendations;
using Dit.EbayClone.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace Dit.EbayClone.Backend.Application.BackgroundWorkers;

public class RecommendationWorker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<RecommendationWorker> logger): BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<RecommendationWorker> _logger = logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                var bids = await dbContext.Bids.ToListAsync(stoppingToken);
                
                // Build indices
                var users = bids.Select(bid => bid.UserId).Distinct().ToList();
                var auctions = bids.Select(bid => bid.AuctionId).Distinct().ToList();
                
                var userIndex = users.Select((id, idx) => new {id, idx})
                    .ToDictionary(x => x.id, x => x.idx);
                
                var auctionIndex = auctions.Select((id, idx) => new {id, idx})
                    .ToDictionary(x => x.id, x => x.idx);
                
                // Build training data
                var ratings = bids
                    .Select(b => new RecommendationArguments() 
                        {
                            userIdx = userIndex[b.UserId],
                            auctionIdx =  auctionIndex[b.AuctionId],
                            rating = b.Amount
                        }
                    ).ToList();
                
                var mf = new MatrixFactorizationAlgorithm(users.Count, auctions.Count);
                mf.Train(ratings, 50);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}