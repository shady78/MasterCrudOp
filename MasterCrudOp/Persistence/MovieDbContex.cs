using MasterCrudOp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MasterCrudOp.Persistence;

public class MovieDbContex(DbContextOptions<MovieDbContex> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("app");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieDbContex).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
       .UseAsyncSeeding(async (context, _, cancellationToken) =>
       {
           var sampleMovie = await context.Set<Movie>().FirstOrDefaultAsync(m => m.Title == "Sonic");
           if (sampleMovie is null)
           {
               sampleMovie = Movie.Create
               (
                   "Sonic",
                   "Fantasy",
                   new DateTimeOffset(new DateTime(2026, 2, 3), TimeSpan.Zero),
                   7
                );
               await context.Set<Movie>().AddAsync(sampleMovie);
               await context.SaveChangesAsync();
           }
       })
       .UseSeeding((context, _) =>
       {
           var sampleMovie = context.Set<Movie>().FirstOrDefault(m => m.Title == "Sonic");
           if (sampleMovie is null)
           {
               sampleMovie = Movie.Create
               (
                   "Sonic",
                   "Fantasy",
                   new DateTimeOffset(new DateTime(2026, 2, 3), TimeSpan.Zero),
                   7
                );
                context.Set<Movie>().Add(sampleMovie);
               context.SaveChanges();
           }
       });
        base.OnConfiguring(optionsBuilder);
    }
}
