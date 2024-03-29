﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesCollection
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public const string DATABASE_NAME = "movies.db";
        public static MoviesRepository database;
        public static MoviesRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new MoviesRepository(DATABASE_NAME);
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
