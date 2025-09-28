namespace Dit.EbayClone.Backend.Core.Options;

public class EmailOptions
{
    public const string Section = "EmailOptions";
    
    public string SmtpServer { get; set; } = string.Empty;
    public string SmtpPort { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromPassword { get; set; } = string.Empty;
}