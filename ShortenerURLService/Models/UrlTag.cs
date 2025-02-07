using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace ShortenerURLService.Models;

[Collection(nameof(UrlTag))]
public class UrlTag
{
    public ObjectId Id { get; set; }
    public required string DestinationUrl { get; set; }
    public required string ShortCode { get; set; }
    public DateTime ExpiryDate { get; set; }
}

