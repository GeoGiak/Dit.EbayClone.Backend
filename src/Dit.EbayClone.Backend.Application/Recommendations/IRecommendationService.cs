namespace Dit.EbayClone.Backend.Application.Recommendations;

public interface IRecommendationService
{
    Task<IReadOnlyList<Guid>> GetREcommendationsAsync(Guid userId, int topN, CancellationToken ct);
}