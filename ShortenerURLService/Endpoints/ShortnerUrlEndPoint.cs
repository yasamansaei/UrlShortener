using Microsoft.AspNetCore.Mvc;
using ShortenerURLService.Infrastructures;
using ShortenerURLService.Infrastructures.Metrics;
using ShortenerURLService.Services;
using ShortenerURLService.Utilities;

namespace ShortenerURLService.Endpoints;
public class UrlRequest
{
    public string Url { get; set; }
}
public static class ShortnerUrlEndPoint
{
    public static void MapShortnerUrl(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/GetShortenerUrl", async (
            ShortenerUrlContext context
            , ShortenerUrlService _shortenerUrlService
            , CancellationToken cancellationToken
            ,[FromBody] UrlRequest UrlRequest) =>
        {

            var result = Validations.ValidateUrl(UrlRequest.Url);

            if (!result.IsNotValid)
            {
                var shortUrl = await _shortenerUrlService.GetUrlCodeAsync(UrlRequest.Url, cancellationToken);
                return Results.Ok(shortUrl);
            }
            else
            {
                return Results.BadRequest(result.ErrorMessage);
            }
        });
    }

    //public static void MapMetricUrl(this IEndpointRouteBuilder endpoint)
    //{
    //    endpoint.MapGet("/GetMtrics",(ShortenerUrlService shortenerUrl)=>
    //    {
    //        var tt = shortenerUrl.GetMetrics();
    //        return Results.Ok(new { shortCode=tt.shor,RedirectUrl=tt.redirect});
    //    });
    //}
}