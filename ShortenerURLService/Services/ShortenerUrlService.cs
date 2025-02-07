using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ShortenerURLService.Infrastructures;
using ShortenerURLService.Infrastructures.Metrics;
using ShortenerURLService.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShortenerURLService.Services
{
    public class ShortenerUrlService(ShortenerUrlContext context,IConfiguration configuration, ShortenerUrlDiagnostics shortenerUrlDiagnostics)
    {
        private readonly ShortenerUrlContext context = context;
        private readonly IConfiguration _configuration = configuration;

        private static int _shortCode;
        private static int _redirectUrl;

        public ShortenerUrlDiagnostics _shortenerUrlDiagnostics { get; } = shortenerUrlDiagnostics;

        public (int shor,int redirect) GetMetrics() => new(_shortCode,_redirectUrl );
        public async Task<string> GetLongUrlAsync(string ShortenerCode, CancellationToken cancellationToken)
        {
            try
            {
                _shortenerUrlDiagnostics.AddRedirect();
                var result = await context.UrlTags.FirstOrDefaultAsync(x => x.ShortCode == ShortenerCode);
                if (result != null)
                {
                    return result.DestinationUrl;
                }
                else
                {
                    _shortenerUrlDiagnostics.AddExceptionDirect();
                    return string.Empty;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> GetUrlCodeAsync(string Url,CancellationToken cancellationToken)
        {
            try
            {
                _shortenerUrlDiagnostics.AddShortenerCode();
                var findUrl = await context.UrlTags.FirstOrDefaultAsync(x => x.DestinationUrl==Url, cancellationToken);
                if (findUrl != null) return GetServiceUrl(findUrl.ShortCode);
                var shorCode = ShortenUrl(Url);
                var urlTag = new UrlTag()
                {
                    DestinationUrl = Url,
                    ExpiryDate = DateTime.Now.AddDays(5),
                    ShortCode = ShortenUrl(Url),
                };
                context.UrlTags.Add(urlTag);
                await context.SaveChangesAsync(cancellationToken);

                return GetServiceUrl(shorCode);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetServiceUrl(string shortUrl) => $"{_configuration["BaseUrl"]}/{shortUrl}";
        private string ShortenUrl(string destinationUrl)
        {
            if (string.IsNullOrEmpty(destinationUrl))
                throw new ArgumentException("URL cannot be null or empty.");

            // Generate a SHA256 hash of the URL
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(destinationUrl));

                // Convert the hash bytes to a Base64 string
                string base64Hash = Convert.ToBase64String(hashBytes);

                // Remove special characters to make it URL-friendly
                string shortUrl = base64Hash.Replace("+", "")
                                            .Replace("/", "")
                                            .Replace("=", "");

                // Limit the length for a shorter URL
                return shortUrl.Substring(0, 8); // Adjust length as needed
            }
        }
    }
}
