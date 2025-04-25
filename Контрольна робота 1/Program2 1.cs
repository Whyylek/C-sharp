using System;


namespace Program2
{
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }

        public Movie(string title, string director, int year)
        {
            Title = title;
            Director = director;
            Year = year;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Director: {Director}, Year: {Year}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Movie other)
                return Title == other.Title && Director == other.Director && Year == other.Year;
            return false;
        }

        public static bool operator ==(Movie a, Movie b)
        {
            return a?.Equals(b) ?? ReferenceEquals(b, null);
        }

        public static bool operator !=(Movie a, Movie b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Director, Year);
        }
    }

    class Program
    {
        static void Main()
        {
            var movies = new[]
            {
                new Movie("Inception", "Christopher Nolan", 2010),
                new Movie("Interstellar", "Christopher Nolan", 2014),
                new Movie("The Matrix", "Wachowski", 1999),
                new Movie("Avengers", "Russo", 2018)
            };

            Console.WriteLine("List of movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine(movie);
            }

            Console.WriteLine("\nEnter a movie (Title, Director, Year):");
            var input = Console.ReadLine().Split(',');
            if (input.Length != 3 || !int.TryParse(input[2].Trim(), out int year))
            {
                Console.WriteLine("Invalid input format.");
                return;
            }

            var searchMovie = new Movie(
                input[0].Trim(),
                input[1].Trim(),
                year
            );

            bool found = movies.Any(movie => movie == searchMovie);
            Console.WriteLine(found ? "Movie found!" : "Movie not found!");

            Console.WriteLine("\nEnter a year to filter movies:");
            if (!int.TryParse(Console.ReadLine(), out int filterYear))
            {
                Console.WriteLine("Invalid year format.");
                return;
            }

            var filteredMovies = movies.Where(movie => movie.Year > filterYear);

            Console.WriteLine("Movies released after the given year:");
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine(movie);
            }
        }
    }
}