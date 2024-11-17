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
        private Dictionary<string, string> urls;
         
        // fav titles
        HashSet<string> favs = new HashSet<string>();


        private Movie display;

        // title to url

        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            this.InitializeComponent();

             LoadMoviesAsync();

            // initialization code
            btnFavoriteBase.Visibility = Visibility.Collapsed;
            tbFavorite.Visibility = Visibility.Collapsed;
            faveCount = 0;

            tbFavoriteRemove.Visibility = Visibility.Collapsed;
            /*btnFavoriteRemove.Visibility = Visibility.Collapsed;
            btnFavoriteRemove.Visibility = Visibility.Collapsed;*/

            // init
            //CreateNewFavorite("my-neighbor-totoro", "https://www.ghiblicollection.com/products/my-neighbor-totoro");
            //CreateNewFavorite("ponyo", "https://www.ghiblicollection.com/products/ponyo-1");

            urls = new Dictionary<string, string>();
            urls.Add("Nausicaä of the Valley of the Wind", "https://ghiblicollection.com/products/nausicaa-of-the-valley-of-the-wind");
            urls.Add("Castle in the Sky", "https://ghiblicollection.com/products/castle-in-the-sky");
            urls.Add("Grave of the Fireflies", "https://www.imdb.com/title/tt0095327/");
            urls.Add("My Neighbor Totoro", "https://ghiblicollection.com/products/my-neighbor-totoro");
            urls.Add("Kiki's Delivery Service", "https://ghiblicollection.com/products/kiki-s-delivery-service");
            urls.Add("Only Yesterday", "https://ghiblicollection.com/products/only-yesterday-2");
            urls.Add("Porco Rosso", "https://ghiblicollection.com/products/porco-rosso-1");
            urls.Add("Ocean Waves", "https://ghiblicollection.com/products/ocean-waves-blu-ray");
            urls.Add("Pom Poko", "https://ghiblicollection.com/products/pom-poko-1");
            urls.Add("Whisper of the Heart", "https://ghiblicollection.com/products/whisper-of-the-heart-1");
            urls.Add("Princess Mononoke", "https://ghiblicollection.com/products/princess-mononoke");
            urls.Add("My Neighbors the Yamadas", "https://ghiblicollection.com/products/my-neighbors-the-yamadas-1");
            urls.Add("Spirited Away", "https://ghiblicollection.com/products/spirited-away-live-on-stage");
            urls.Add("The Cat Returns", "https://ghiblicollection.com/products/the-cat-returns-1");
            urls.Add("Howl's Moving Castle", "https://ghiblicollection.com/products/howl-s-moving-castle-1");
            urls.Add("Tales from Earthsea", "https://ghiblicollection.com/products/tales-from-earthsea-1");
            urls.Add("Ponyo", "https://www.ghiblicollection.com/products/ponyo-1");
            urls.Add("Arrietty", "https://ghiblicollection.com/products/the-secret-world-of-arrietty");
            urls.Add("From Up on Poppy Hill", "https://ghiblicollection.com/products/from-up-on-poppy-hill");
            urls.Add("The Wind Rises", "https://ghiblicollection.com/products/the-wind-rises");
            urls.Add("The Tale of the Princess Kaguya", "https://ghiblicollection.com/products/the-tale-of-the-princess-kaguya-2");
            urls.Add("When Marnie Was There", "https://ghiblicollection.com/products/when-marnie-was-there-3");
            urls.Add("The Red Turtle" , "https://www.imdb.com/title/tt3666024");
            urls.Add("Earwig and the Witch", "https://ghiblicollection.com/products/earwig-and-the-witch");
            urls.Add("The Boy and the Heron", "https://ghiblicollection.com/products/the-boy-and-the-heron");

        }


        private async Task LoadMoviesAsync()
        {
            var response = await client.GetStringAsync("https://ghibliapi.dev/films");
            var movies = JsonSerializer.Deserialize<List<Movie>>(response);

            // update link: name->url

            MovieGrid.ItemsSource = movies; // Binding the data to the grid
        

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

                dialog.ShowAsync();
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
   
                wvMain.Navigate(new Uri(urls[clickedMovie.Title]  ));
                movies.Visibility = Visibility.Collapsed;

                display = clickedMovie;

                tblWelcome.Visibility = Visibility.Collapsed;
                // display wv
                wvMain.Visibility = Visibility.Visible;
                tbSearch.Text = urls[clickedMovie.Title];

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

                tblWelcome.Visibility = Visibility.Collapsed;
                movies.Visibility = Visibility.Collapsed;

                display = null;

                String text = tbSearch.Text.Trim();
                if (text.Length > 0 && text != "Search...")
                {
                    tblWelcome.Visibility = Visibility.Collapsed;
                    string url =  text;
                    wvMain.Navigate(new Uri(url));
                    wvMain.Visibility = Visibility.Visible;
                
                }
            }
        }
        

        private void tbFavorite_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            // if the user hits enter on the favorite textbox, add a favorite button!
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // only let the user add a favorite button if it's not the default text
                // TODO add checks on what they typed, so I at least THINK its a valid url...?
                String text = tbFavorite.Text.Trim();
                if (text.Length > 0 && text != "<Enter A New Favorite>")
                {
                    String url = FixText(text);  // if they type 'imdb' for example, add http..., and .com

                    CreateNewFavorite(text, url); // create the new button
                    tbFavorite.Visibility = Visibility.Collapsed;
                    btnFavorite.Focus(FocusState.Programmatic);
                }
            }
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

        // creates a new favorite button
        private void CreateNewFavorite(String showText, String url)
        {

            if (favs.Contains(showText))
            {
                btnFavorite.Content = "♥︎";
                return;
            }

            favs.Add(showText);

            Button newFavorite = new Button();
            // add the new button to the appropriate grid, for formatting
            gridFavorites.Children.Add(newFavorite);

            newFavorite.Content = showText; // the text
            newFavorite.Tag = url;          // the navigation url

            // the name, used to identify it later
            newFavorite.Name = "fave" + faveCount;

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

            // increment our favorite count!
            faveCount++;
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
        }
        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            // reset the favorite textbox to it's default state
            // tbFavorite.Text = "<Enter A New Favorite>";
            // tbFavorite.Visibility = Visibility.Visible;
            // tbFavorite.SelectAll();
            // tbFavorite.Focus(FocusState.Programmatic);

          

            // add current movie into fav: if wv is in display
            if (wvMain.Visibility == Visibility.Visible && display != null)
            {

                CreateNewFavorite(display.Title, urls[display.Title]); // create the new button
                btnFavorite.Content = "♥︎";
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

        private void tbFavoriteRemove_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            // if the user hits enter on the favorite textbox, add a favorite button!
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // only let the user remove a favorite button if it's not the default text
                // TODO add checks on what they typed, so I at least THINK its a valid url...?
                String text = tbFavorite.Text.Trim();
                if (text.Length > 0 && text != "<Enter A Favorite Name>")
                {
                    //String url = FixText(text);  // if they type 'imdb' for example, add http..., and .com

                    // don't remove the add, base, or move buttons!
                    if (text != "+" && text != "-" && text != "Button")
                    {
                        RemoveFavorite(text); // remove the button
                        tbFavorite.Visibility = Visibility.Collapsed;
                        btnFavorite.Focus(FocusState.Programmatic); // yes, give focus back to ADD favorite
                    }
                }
            }
        }

        private void RemoveFavorite(String text)
        {
            double top = 0.0;
            bool removed = false;
            foreach (var item in gridFavorites.Children)
            {
                if (item is Button b)
                {
                    if (b.Content.ToString() == text)
                    {
                        top = b.Margin.Top;

                        gridFavorites.Children.Remove(item);
                        faveCount--;
                        removed = true;
                        break;
                    }
                }
            }

            if (removed)
            {
                // move each button that is BELOW that, up by the appropriate margin!
                foreach (var item in gridFavorites.Children)
                {
                    if (item is Button b)
                    {
                        String btext = b.Content.ToString();
                        if (btext != "+" && btext != "-" && btext != "Button")
                        {
                            if (b.Margin.Top > top)
                            {
                                b.Margin = new Thickness(
                                    b.Margin.Left,
                                    b.Margin.Top - (b.Height + separation),
                                    b.Margin.Right,
                                    b.Margin.Bottom);
                            }
                        }
                    }
                }
            }
        }

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

    internal class MovieService
    {
            private static readonly HttpClient client = new HttpClient();

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var response = await client.GetStringAsync("https://ghibliapi.dev/films");
        var movies = JsonSerializer.Deserialize<List<Movie>>(response);
        return movies;
    }
    
    }
}
