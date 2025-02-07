using Microsoft.AspNetCore.Mvc;
using ShortenerURLService.Infrastructures;
using ShortenerURLService.Services;

namespace ShortenerURLService.Endpoints;

public static class RedirectUrlEndPoint
{
    public static void MapRedirectUrl(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/{shortener_code}", async (
            ShortenerUrlContext context
            , ShortenerUrlService _shortenerUrlService
            , CancellationToken cancellationToken
            , [FromRoute(Name = "shortener_code")] string shortenerCode) =>
        {
            if (!string.IsNullOrEmpty(shortenerCode))
            {
                var longUrl = await _shortenerUrlService.GetLongUrlAsync(shortenerCode, cancellationToken);
                return string.IsNullOrEmpty(longUrl) ? Results.NotFound() : Results.Redirect(longUrl);// Results.Ok(shortUrl);
            }
            else
            {
                return Results.BadRequest();
            }
        });
    }
}
