using System;
using Xamarin.Forms;
using System.IO;
using MoviesCollection.iOS;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace MoviesCollection.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            //путь к БД
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); //папка библиотеки
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}