namespace ShortenerURLService.Utilities
{
    public static class Validations
    {
        public static (bool IsNotValid, string ErrorMessage) ValidateUrl(string destinationUrl)
        {
            string errorMessage = "";
            if (string.IsNullOrWhiteSpace(destinationUrl))
                return (true, "URL cannot be null or empty.");



            if (!Uri.TryCreate(destinationUrl, UriKind.Absolute, out Uri uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                return (true, "Invalid URL format. Only HTTP and HTTPS are supported.");

            if (destinationUrl.Length > 2000)
                return (true, "URL length exceeds the maximum allowed limit.");

            if (destinationUrl.Contains("javascript:", StringComparison.OrdinalIgnoreCase) ||
                destinationUrl.Contains("<script", StringComparison.OrdinalIgnoreCase))
                return (true, "URL contains potentially malicious content.");

            string[] forbiddenCharacters = { ";", "|", "&&", "%", "<", ">" };
            if (forbiddenCharacters.Any(destinationUrl.Contains))
                return (true, "URL contains forbidden characters.");

            //if (uriResult.Host == "localhost" || uriResult.Host.EndsWith(".local"))
            //    errorMessage="URLs pointing to localhost are not allowed.";

            string[] notTrustedDomains = { "example.com", "trusted.com" };
            if (!Uri.TryCreate(destinationUrl, UriKind.Absolute, out Uri uriResult2) ||
                notTrustedDomains.Any(domain => uriResult2.Host.EndsWith(domain, StringComparison.OrdinalIgnoreCase)))
                return (true, "URL domain is not trusted.");

            return (false, errorMessage);

        }
    }
}
