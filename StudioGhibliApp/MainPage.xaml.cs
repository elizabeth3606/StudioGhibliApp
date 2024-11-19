using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
 using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StudioGhibliApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static int separation = 10;
        // the number of favorites buttons i've added
        private int faveCount;
        private String stringUrl;

        private List<Movie> all;

        // fav titles
        HashSet<string> favs = new HashSet<string>();


        private Movie display;

        // title to url
 
        public MainPage()
        {
            this.InitializeComponent();

             LoadMoviesAsync();


            // initialization code
            btnFavoriteBase.Visibility = Visibility.Collapsed;
            //tbFavorite.Visibility = Visibility.Collapsed;
            faveCount = 0;

            //tbFavoriteRemove.Visibility = Visibility.Collapsed;
            /*btnFavoriteRemove.Visibility = Visibility.Collapsed;
            btnFavoriteRemove.Visibility = Visibility.Collapsed;*/


        }


        private async void LoadMoviesAsync()
        {
            // var response = await client.GetStringAsync("https://ghibliapi.dev/films");
            // var movies = JsonSerializer.Deserialize<List<Movie>>(response);

            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/movies.json"));
 
            string jsonText = await FileIO.ReadTextAsync(jsonFile);
            // Optionally, you can deserialize the JSON into an object
            //  var jsonData = JsonConvert.DeserializeObject<MyData>(jsonText);

            // var response = await client.GetStringAsync("https://ghibliapi.dev/films");
            all  = JsonSerializer.Deserialize<List<Movie>>(jsonText);


            MovieGrid.ItemsSource = all; // Binding the data to the grid
        

        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            
            ContentDialog dialog = new ContentDialog
            {
                Title = "Home Button",
                Content = "Going to Home!",
                CloseButtonText = "OK"
            };

            /*dialog.ShowAsync();*/
            tblWelcome.Visibility = Visibility.Visible;
            wvMain.Visibility = Visibility.Collapsed;
            movies.Visibility = Visibility.Visible;

            display = null;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            if (wvMain.CanGoBack)
            {
                wvMain.GoBack();
            }
            else
            {
                // Optionally show a message if there are no pages to go back to
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Back Button",
                    Content = "No pages to go back to.",
                    CloseButtonText = "OK"
                };

                _ = dialog.ShowAsync();
            }
        }
        private void MovieGrid_ItemClick(object sender, ItemClickEventArgs e)

        {
            if (e.ClickedItem is Movie clickedMovie)
            {
                // Handle the click event, e.g., toggle state or show details
                clickedMovie.IsSelected = !clickedMovie.IsSelected;

                // Log or update the UI
                Debug.WriteLine($"Movie clicked: {clickedMovie.Title}, Selected: {clickedMovie.IsSelected}");
   
                wvMain.Navigate(new Uri(clickedMovie.Url));
                movies.Visibility = Visibility.Collapsed;

                display = clickedMovie;

                tblWelcome.Visibility = Visibility.Collapsed;
                // display wv
                wvMain.Visibility = Visibility.Visible;
                tbSearch.Text = clickedMovie.Url;

                if (favs.Contains(clickedMovie.Title))
                {
                    btnFavorite.Content = "♥︎";
                
                } else
                {
                    btnFavorite.Content   = "♡";
                }

            }

            // pull url, display

            // hide grid

        }


        private void tbSearch_Return(object sender, KeyRoutedEventArgs e)
        {
            // if the user hits enter on the favorite textbox, add a favorite button!
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // only let the user add a favorite button if it's not the default text
                // TODO add checks on what they typed, so I at least THINK its a valid url...

                String text = tbSearch.Text.Trim();
                if (text.Length > 0 && text != "Search...")
                {
                    tblWelcome.Visibility = Visibility.Collapsed;
                    string url = FixUrl(text);
                    if (IsValidUrl(url) && url.EndsWith(".com"))
                    {
                        wvMain.Navigate(new Uri(url));
                        wvMain.Visibility = Visibility.Visible;
                        btnFavorite.Content = "♡";

                        tblWelcome.Visibility = Visibility.Collapsed;
                        movies.Visibility = Visibility.Collapsed;

                        display = null;
                        return;
                    }
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Search ",
                        Content = "invalid url: " + url,
                        CloseButtonText = "OK"
                    };

                    _ = dialog.ShowAsync();

                }
            }
        }

        public  bool IsValidUrl(string url)
        {
            // Trim any leading/trailing whitespace
            url = url.Trim();

            // Check if the URL matches a basic regex pattern
            //         var regex = new Regex(@"^(https?://)?([a-z0-9-]+\.)+[a-z]{2,6}(:[0-9]{1,4})?(/.*)?$", RegexOptions.IgnoreCase);

            var regex = new Regex(@"^(https?://)?([a-z0-9-]+\.)+[a-z]{2,6}(:[0-9]{1,4})?(/.*)?$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(url))
            {
                return false;
            }

            try
            {
                // Check if the URL is complete (if missing scheme, assume 'http')
                Uri uri;
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    // If missing scheme (like "www.google.com"), add "http://"
                    url = "http://" + url;
                }

                uri = new Uri(url);  // Try to create a Uri object
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps; // Must be HTTP or HTTPS
            }
            catch (UriFormatException)
            {
                return false;
            }
        }


 

        private String FixUrl(String url)

        {
         
            if (!url.EndsWith(".com"))
            {
                url = url + ".com";
            }
            
            if (url.StartsWith("https://") && !url.StartsWith("https://www."))
            {
                url = url.Substring(0, 8) + "www." + url.Substring(8);
            }
            else if (url.StartsWith("www."))
            {
                url = "https://" + url;
            }
            else
            {
                url = "https://www." + url;
            }

            return url.ToLower(); 

        }

        private String FixText(String text)
        {
            String textLower = text.ToLower();
            text = textLower.Replace(" ", "-");
            string url = "https://www.ghiblicollection.com/products/" + text;
            /*if (textLower.StartsWith("https://") && !textLower.StartsWith("https://www."))
            {
                text = text.Substring(0, 8) + "www." + text.Substring(8);
            }
            else if (textLower.StartsWith("www."))
            {
                text = "https://" + text;
            }
            else
            {
                text = "https://www." + text;
            }


            if (!textLower.EndsWith(".org") && !textLower.EndsWith(".com"))
            {
                text = text + ".com";
            }*/
            this.stringUrl = url;
            return url;
        }

        private void RefreshFavs() {
            // for each fav, add button
            var buttonsToRemove = gridFavorites.Children.OfType<Button>()
                                     .Where(b => b.Name != "btnFavoriteBase")
                                     .ToList();

            // Remove the buttons
            foreach (var button in buttonsToRemove)
            {
                gridFavorites.Children.Remove(button);
            }

            // add according to favs
            faveCount = 0;

            foreach (string s in favs)
            {
                Button newFavorite = new Button();
                // add the new button to the appropriate grid, for formatting
                gridFavorites.Children.Add(newFavorite);

                newFavorite.Content = s; // the text
                Movie movie = all.FirstOrDefault(m => m.Title.Equals(s, StringComparison.OrdinalIgnoreCase));
                newFavorite.Tag = movie.Url;           // the navigation url

                // the name, used to identify it later
                newFavorite.Name = s;

                // style of the size
                newFavorite.Height = btnFavoriteBase.Height;
                newFavorite.Width = btnFavoriteBase.Width;
                newFavorite.Style = btnFavoriteBase.Style;

                // the font
                newFavorite.FontFamily = btnFavoriteBase.FontFamily;
                newFavorite.FontSize = btnFavoriteBase.FontSize;
                newFavorite.FontStyle = btnFavoriteBase.FontStyle;
                newFavorite.FontWeight = btnFavoriteBase.FontWeight;

                newFavorite.Margin = btnFavoriteBase.Margin;
                newFavorite.Margin = new Thickness(
                btnFavoriteBase.Margin.Left,
                    btnFavoriteBase.Margin.Top + (faveCount * (newFavorite.Height + separation)),
                    btnFavoriteBase.Margin.Right,
                    btnFavoriteBase.Margin.Bottom - (faveCount * (newFavorite.Height + separation)));
                newFavorite.Click += FavoriteClick;
                newFavorite.Visibility = Visibility.Visible;
                faveCount++;

            }
        }

        // creates a new favorite button
        private void CreateNewFavorite(String showText, String url)
        {

            if (favs.Contains(showText))
            {
                btnFavorite.Content = "♥︎";
                return;
            }

            favs.Add(showText);
            RefreshFavs();

        }

        // navigate to the appropriate url when they click favorite!
        private void FavoriteClick(object sender, RoutedEventArgs e)
        {

      
 
            String url = ((Button)sender).Tag.ToString();
            wvMain.Navigate(new Uri(url));
            tbSearch.Text = url;
            wvMain.Visibility = Visibility.Visible;
            btnFavorite.Content = "♥︎";
            tblWelcome.Visibility = Visibility.Collapsed;

         
            display = all.FirstOrDefault(m => m.Title == ((Button)sender).Content);
        }
        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
 
          

            // add current movie into fav: if wv is in display
            if (wvMain.Visibility == Visibility.Visible && display != null && (string) btnFavorite.Content == "♡")
            {

                CreateNewFavorite(display.Title, display.Url); // create the new button
                btnFavorite.Content = "♥︎";
                RefreshFavs();
            } else if (wvMain.Visibility == Visibility.Visible && display != null && (string)btnFavorite.Content == "♥︎")
            {
                favs.Remove(display.Title);
                btnFavorite.Content = "♡";
                RefreshFavs();
            }

        }
        /*private void btnFavoriteRemove_Click(object sender, RoutedEventArgs e)
        {
            // reset the favorite textbox to it's default state
            tbFavoriteRemove.Text = "<Enter A Favorite Name>";
            tbFavoriteRemove.Visibility = Visibility.Visible;
            tbFavoriteRemove.SelectAll();
            tbFavoriteRemove.Focus(FocusState.Programmatic);
        }*/
 
  
         private void btnFore_Click(object sender, RoutedEventArgs e)
        {
            if (wvMain.CanGoForward)
            {
                wvMain.GoForward();
            }
            else
            {
                // Optionally show a message if there are no pages to go back to
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Foreward Button",
                    Content = "No pages to go to.",
                    CloseButtonText = "OK"
                };

                _ = dialog.ShowAsync();
            }
        }

        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
 


        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = this.stringUrl;
        }

        /*private void tbFavoriteRemove_TextChanged(object sender, TextChangedEventArgs e)
        {

        }*/
    }


}
