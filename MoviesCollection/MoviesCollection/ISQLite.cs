using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesCollection
{
    public interface ISQLite
    {
        string GetDatabasePath(string filename);
    }
}
