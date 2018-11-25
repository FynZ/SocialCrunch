using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;

namespace Data
{
    internal abstract class BaseRepository
    {
        protected virtual IDbConnection Connection => new NpgsqlConnection("_connectionString");
    }
}
