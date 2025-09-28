using System.ComponentModel.DataAnnotations;

namespace Dit.EbayClone.Backend.Core.Options;

public class JwtOptions
{
    public const string Section = "JwtOptions";
    
    [Required]
    public string Audience { get; set; } = string.Empty;
    
    [Required]
    public string Issuer { get; set; } = string.Empty;
    
    [Required]
    public string ExpirationMinutes { get; set; } = string.Empty;
    
    [Required]
    public string SigningKey { get; set; } = string.Empty;
}