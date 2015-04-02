using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Reviews.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Reviews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : BasePage
    {
        readonly HttpClient _client = new HttpClient();

        public SearchPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }


        private void TxtSuggestion_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            lstItems.ItemsSource = null;
          var text=  txtSuggestion.Text;
            Task.Run(async () =>
            {
               
                if (!string.IsNullOrEmpty(text))
                {
                    try
                    { 
                        _client.CancelPendingRequests();
                        var asf = await _client.GetStringAsync(
                            string.Format(
                                "http://cdn.marketplaceedgeservice.windowsphone.com/v8/catalog/queries?os=8.10.14219.0&zLocale=EN-CA&cc=US&lang=en-US&prefix={0}&chunksize=4&includeApplications=true&includeAlbums=fal",
                                text));
                        var doc = XDocument.Parse(asf);
                        XmlSerializer serializer = new XmlSerializer(typeof(QueryResult));
                        var feed = (QueryResult)serializer.Deserialize(doc.CreateReader());
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () => txtSuggestion.ItemsSource = feed.Entry.Select(entry => entry.Title));
                    }
                    catch (TaskCanceledException)
                    {

                    }
                    catch
                    {
                    }
                }
                else
                {
                   Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () => txtSuggestion.ItemsSource = null);
                }

            });

        }

        private void TxtSuggestion_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Find(args.SelectedItem.ToString());
        }

        private async void Find(string word)
        {
            try
            {
                HttpClient client = new HttpClient();
                txtSuggestion.IsSuggestionListOpen = false;
                var asf = await client.GetStringAsync(
                    string.Format(
                        "http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&chunkSize=50&q={0}&cf=99-1",
                        word));
                var doc = XDocument.Parse(asf);
                XmlSerializer serializer = new XmlSerializer(typeof(SearchResult));
                var feed = (SearchResult)serializer.Deserialize(doc.CreateReader());
                lstItems.ItemsSource = feed.Entry;
                lstItems.Focus(FocusState.Programmatic);
            }
            catch (Exception)
            {
               
            }
          
        }

        private void TxtSuggestion_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(txtSuggestion.Text))
            {
                Find(txtSuggestion.Text);
            }
        }

        private void LstItems_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var entry = e.ClickedItem as Entry;
            LocalCacheHelper.AddRecentApp(entry);
            var frame = Window.Current.Content as Frame;
            if (frame != null && entry != null) frame.Navigate(typeof(AppDetailsPage), entry.Id);
        }

        protected override void NavigationHelper_LoadState(object sender, CinelabWP8_1.Common.LoadStateEventArgs e)
        {

        }

        protected override void NavigationHelper_SaveState(object sender, CinelabWP8_1.Common.SaveStateEventArgs e)
        {

        }

        protected override bool ChildAllowedToExit(Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            return false;
        }

        private void TxtSuggestion_OnLoaded(object sender, RoutedEventArgs e)
        {
            //(sender as Control).Focus(FocusState.Keyboard);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtSuggestion.Focus(FocusState.Keyboard);
        }
    }
}
