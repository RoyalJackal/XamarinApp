using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using XamarinApp.Data;

namespace XamarinApp
{
    public partial class App : Application
    {
        private static Database db;

        public static SQLiteAsyncConnection Db
        {
            get
            {
                if (db == null)
                {
                    db = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return db.database;
            }
        }


        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
