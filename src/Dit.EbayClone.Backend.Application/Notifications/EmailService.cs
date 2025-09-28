using System.Net;
using System.Net.Mail;
using Dit.EbayClone.Backend.Core.Options;
using Dit.EbayClone.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dit.EbayClone.Backend.Application.Notifications;

public class EmailService(
    IOptions<EmailOptions> emailOptions,
    ApplicationDbContext dbContext,
    ILogger<EmailService> logger): INotificationService
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<EmailService> _logger = logger;
    
    public async Task SendNotificationAsync(Guid userId, string title, string message, CancellationToken cancellationToken)
    {
        var recipientEmail = await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipientEmail == null)
        {
            _logger.LogWarning("User with id {UserId} not found", userId);
            return;
        }
        
        using var client = new SmtpClient(_emailOptions.SmtpServer, int.Parse(_emailOptions.SmtpPort));
        client.Credentials = new NetworkCredential(_emailOptions.FromEmail, _emailOptions.FromPassword);
        client.EnableSsl = true;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailOptions.FromEmail),
            Subject = title,
            Body = message,
            IsBodyHtml = true,
        };
        
        mailMessage.To.Add(recipientEmail);
        
        await client.SendMailAsync(mailMessage, cancellationToken);
    }
}