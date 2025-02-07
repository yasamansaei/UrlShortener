using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using ShortenerURLService.Models;

namespace ShortenerURLService.Infrastructures;

public class ShortenerUrlContext:DbContext
{
    public ShortenerUrlContext(DbContextOptions<ShortenerUrlContext> options) : base(options)
    {
    }

    public DbSet<UrlTag> UrlTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
