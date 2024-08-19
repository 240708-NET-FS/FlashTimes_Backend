namespace FlashTimes.Utilities;

public class JwtSettings
{
    public string? Secret { get; set; }
    public int ExpiryInMinutes { get; set; }
}
