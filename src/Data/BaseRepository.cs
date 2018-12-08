using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;

namespace Data
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected virtual IDbConnection Connection => new NpgsqlConnection(_connectionString);
    }
}
