using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinApp.Models;

namespace XamarinApp.Data
{
    public class Database
    {
        public readonly SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Fodder>();
            database.CreateTableAsync<Pet>();
            database.CreateTableAsync<Feed>();
        }
    }
}
