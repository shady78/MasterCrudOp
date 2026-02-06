using System.ComponentModel.DataAnnotations;

namespace MasterCrudOp.Models;

public sealed class Movie : EntityBase
{
    public string Title { get; private set; }
    public string Genre { get; private set; }
    public DateTimeOffset ReleasedDate { get; private set; }
    public double Rating { get; private set; }
    private Movie()
    {
        Title = string.Empty;
        Genre = string.Empty;
    }
    private Movie(string title, string genre , DateTimeOffset releasedDate, double rating)
    {
        Title = title;
        Genre = genre;
        ReleasedDate = releasedDate;
        Rating = rating;
    }
    
    public static Movie Create(string title, string genre , DateTimeOffset releasedDate, double rating)
    {
        ValidateInputs(title, genre, releasedDate, rating);
        return new Movie(title, genre, releasedDate, rating);
    }

    public void Update(string title, string genre , DateTimeOffset releasedDate, double rating)
    {
        ValidateInputs(title, genre, releasedDate, rating);
        Title = title;
        Genre = genre;
        ReleasedDate = releasedDate;
        Rating = rating;

        UpdateLastModified();
    }
    private static void ValidateInputs(string title, string genre, DateTimeOffset releaseDate, double rating)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre cannot be null or empty.", nameof(genre));

        if (releaseDate > DateTimeOffset.UtcNow)
            throw new ArgumentException("Release date cannot be in the future.", nameof(releaseDate));

        if (rating < 0 || rating > 10)
            throw new ArgumentException("Rating must be between 0 and 10.", nameof(rating));
    }
}
