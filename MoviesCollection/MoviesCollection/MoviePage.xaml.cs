using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesCollection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePage : ContentPage
    {
        //Added to let user delete genres. It'll be the first default value for Genre Picker to manifest absense of a genre
        private static readonly Genre NullGenre = new Genre() { Name = "Без жанра" };
        private static readonly Genre[] GENRES = new Genre[] { NullGenre }.Concat(App.Database.GetGenres()).ToArray();

        public MoviePage()
        {
            InitializeComponent();
        }

        private void SaveMovie(object sender, EventArgs e)
        {
            var movie = (Movie) BindingContext;
            if (!String.IsNullOrEmpty(movie.Name))
            {
                App.Database.SaveMovie(movie, GetSelectedGenres());
            }
            this.Navigation.PopAsync();
        }

        //creating list of all selected genres
        private Genre[] GetSelectedGenres()
        {
            if (GenresStackLayout.Children.Count > 1)
            {
                var selectedGenres = new Genre[GenresStackLayout.Children.Count - 1];
                for (int i = 0; i < GenresStackLayout.Children.Count - 1; i++)
                {
                    selectedGenres[i] = (Genre)((Picker)GenresStackLayout.Children[i]).SelectedItem;
                }
                return selectedGenres;
            }
            return null;
        }

        private void DeleteMovie(object sender, EventArgs e)
        {
            var movie = (Movie) BindingContext;
            App.Database.DeleteMovie(movie.Id);
            this.Navigation.PopAsync();
        }
        private void Cancel(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            var movie = (Movie)BindingContext;

            //delete button is available only on opening already exisiting movie
            DeleteBtn.IsVisible = movie.Name != null;

            //filling with Pickers for every genre
            foreach (var genre in App.Database.GetGenres(movie.Id))
            {
                var picker = CreateGenrePicker();
                GenresStackLayout.Children.Insert(0, picker);
                picker.SelectedItem = genre;
            }
       
            //if there is no Name, then nothing to save
            SaveBtn.IsEnabled =  AddGenreBtn.IsEnabled = !String.IsNullOrEmpty(((Movie)BindingContext).Name);

            base.OnAppearing();
        }

        //create and open new Picker for new genre
        private void AddGenre(object sender, EventArgs e)
        {
            var genrePicker = CreateGenrePicker();
            GenresStackLayout.Children.Insert(0, genrePicker);
            genrePicker.Focus();
        }

        private Picker CreateGenrePicker()
        {
            var picker = new Picker();
            picker.ItemsSource = GENRES;
            //what should width be?
            picker.WidthRequest = 100;
            picker.SelectedIndexChanged += CloseGenrePicker;
            return picker;
        }

        private void CloseGenrePicker(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            //if first, no genre, option is selected, then remove Picker
            if (picker.SelectedItem == NullGenre)
            {
                GenresStackLayout.Children.Remove(picker);
            }
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveBtn.IsEnabled = AddGenreBtn.IsEnabled = !String.IsNullOrEmpty(((Entry)sender).Text);
        }
    }
}