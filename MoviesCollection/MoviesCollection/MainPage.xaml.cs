using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MoviesCollection
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            moviesList.ItemsSource = App.Database.GetMovies();
        }
     
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedMovie = (Movie) e.SelectedItem;
            var moviePage = new MoviePage();
            moviePage.BindingContext = selectedMovie;
            await Navigation.PushAsync(moviePage);
        }

        private async void CreateMovie(object sender, EventArgs e)
        {
            var movie = new Movie();
            MoviePage addMoviePage = new MoviePage();
            addMoviePage.BindingContext = movie;
            await Navigation.PushAsync(addMoviePage);
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = ((Entry)sender).Text;
            if (String.IsNullOrEmpty(searchText))
                moviesList.ItemsSource = App.Database.GetMovies();
            else
                moviesList.ItemsSource = App.Database.SearchMovies(searchText);
            
        }
    }
}
