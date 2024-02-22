using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class WeatherContext : DbContext
{
    public DbSet<Weather> Weathers { get; set; }

    public WeatherContext(DbContextOptions<WeatherContext> options)
        : base(options) { }
}