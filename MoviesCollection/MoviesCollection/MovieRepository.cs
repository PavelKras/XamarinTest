using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MoviesCollection
{
    public class MoviesRepository
    {
        private SQLiteConnection movieDatabase;
        private static readonly string[] GENRES = {"Комедия", "Приключения", "Фантастика", "Драма", "Боевик", "Военный", "Мелодрама" };
        private static readonly string[]MOVIES = { "Покровские ворота", "Дети капитана Гранта", "Акванавты", "Премия", "Пираты XX века", "Горячий снег", "Служебный роман" };
        public MoviesRepository(string filename)
        {
            string movieDatabasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            movieDatabase = new SQLiteConnection(movieDatabasePath);
            //movieDatabase.DropTable<Movie>();
            //movieDatabase.DropTable<Genre>();
            //movieDatabase.DropTable<MovieGenre>();
            movieDatabase.CreateTable<Movie>();
            movieDatabase.CreateTable<Genre>();
            movieDatabase.CreateTable<MovieGenre>();
            
            if (movieDatabase.Table<Genre>().Count() == 0)
            {
                foreach (string genreName in GENRES)
                {
                    movieDatabase.Execute($"INSERT INTO Genres (Name) VALUES ('{genreName}')");
                }
                foreach (string movieName in MOVIES)
                {
                    movieDatabase.Execute($"INSERT INTO Movies (Name) VALUES ('{movieName}')");
                }
            }
        }

        public IEnumerable<Movie> GetMovies()
        {
            return movieDatabase.Table<Movie>();
        }

        public Movie GetMovie(int id)
        {
            return movieDatabase.Get<Movie>(id);
        }

        public int DeleteMovie(int id)
        {
            return movieDatabase.Delete<Movie>(id);
        }

        public void SaveMovie(Movie movie)
        {
            if (movie == null)
                return;
            if (movie.Id != 0)
            {
                movieDatabase.Update(movie);
            }
            else
            {
                movieDatabase.Insert(movie);
            }
        }

        public void SaveMovie(Movie movie, Genre[] genres)
        {
            if (movie == null)
                return;

            SaveMovie(movie);
            //If empty or null genres were passed then we just delete all genres for this movie
            movieDatabase.Execute($"DELETE FROM MovieGenre WHERE MovieId = {movie.Id}");
            if (genres != null)
            {
                foreach (var genre in genres)
                {
                    if (genre != null)
                        SaveMovieGenre(new MovieGenre() { MovieId = movie.Id, GenreId = genre.Id });
                }
            }
        }

        public void SaveMovieGenre(MovieGenre movieGenre)
        {
            movieDatabase.Insert(movieGenre);
        }

        public IEnumerable<Genre> GetGenres()
        {
            return movieDatabase.Table<Genre>();
        }

        public IEnumerable<Genre> GetGenres(int movieId)
        {
            var genres = movieDatabase.Table<MovieGenre>().Where(mg => mg.MovieId == movieId).Select(mg => mg.GenreId);
            return movieDatabase.Table<Genre>().Where(g => genres.Contains(g.Id));
        }

        public IEnumerable<Movie> SearchMovies(string searchStr)
        {
            searchStr = searchStr.ToLower();
            //space for optimization: seems like a lot of DB queries

            //search phrase in movie names and descriptions
            var movies = movieDatabase.Table<Movie>().AsEnumerable().Where(
                m => m.Name.ToLower().Contains(searchStr) ||
                (m.Description != null ? m.Description.ToLower().Contains(searchStr) : false));

            //search phrase in genre names
            var genres = movieDatabase.Table<Genre>().AsEnumerable().Where(m => m.Name.ToLower().Contains(searchStr));
            var movieIdsFromGenres = movieDatabase.Table<MovieGenre>().AsEnumerable().
                Where(mg => genres.Any(g => g.Id == mg.GenreId)).Select(mg => mg.MovieId);
            var moviesFromGenres = movieIdsFromGenres.Select(mfg => GetMovie(mfg));

            return movies.Concat(moviesFromGenres).Distinct();
        }
    }
}
