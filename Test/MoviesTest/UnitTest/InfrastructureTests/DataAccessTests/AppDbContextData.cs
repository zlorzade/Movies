using Movies.Core;
using System;
using System.Collections.Generic;

namespace DataAccess.UnitTest;

public static class AppDbContextData
{
    public static ICollection<Genre> Genres => GetGenres();

    private static ICollection<Genre> GetGenres()
    {
        List<Genre> genres = new();

        Genre genre1 = new()
        {
            Name = "اکشن",
            Code = 1001,
            IsActive = true,
            Priority = 1,
            description = "ژانر اکشن",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre1);

        Genre genre2 = new()
        {
            Name = "درام",
            Code = 1002,
            IsActive = false,
            Priority = 2,
            description = "ژانر درام",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre2);

        Genre genre3 = new()
        {
            Name = "کمدی",
            Code = 1005,
            IsActive = true,
            Priority = 3,
            description = "ژانر کمدی",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre3);

        return genres;
    }

    public static ICollection<Movie> Movies => GetMovies();

    private static ICollection<Movie> GetMovies()
    {
        List<Movie> genres = new();

        Movie genre1 = new()
        {
            Name = "سریع و خشن",
            Code = 100,
            IsActive = true,
            GenreId = 1,
            description = "بسیار مهیج",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre1);

        Movie genre2 = new()
        {
            Name = "جان ویک",
            Code = 103,
            IsActive = false,
            GenreId = 1,
            description = "پرفروش ترین اکشن اخیر",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre2);

        Movie genre3 = new()
        {
            Name = "اتو",
            Code = 108,
            IsActive = true,
            GenreId = 2,
            description = "یک پیرمرد تنها",
            LastUpdatedDate = DateTime.Now,
        };
        genres.Add(genre3);

        return genres;
    }
}