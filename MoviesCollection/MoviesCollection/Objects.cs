using SQLite;

namespace MoviesCollection
{
    public abstract class Item
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Item;
            if (item == null)
                return false;
            return Id == item.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

    [Table("Movies")]
    public class Movie : Item
    {
        [NotNull]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    [Table("Genres")]
    public class Genre : Item
    {
        [NotNull, Unique]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Table("MovieGenres")]
    public class MovieGenre : Item
    {
        public int MovieId { get; set; }

        public int GenreId { get; set; }
    }
}
