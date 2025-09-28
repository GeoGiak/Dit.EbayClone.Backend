namespace Dit.EbayClone.Backend.Application.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(Guid userId, string title, string message, CancellationToken cancellationToken);
}