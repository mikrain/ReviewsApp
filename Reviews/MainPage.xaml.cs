using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Reviews.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Reviews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;


            //icon http://cdn.marketplaceimages.windowsphone.com/v8/images/2f9c349d-e43e-42ae-a890-137154dddf3a?imageType=ws_icon_large
            //another details https://services.apps.microsoft.com/browse/6.2.9200-1/615/en-US_en-US/c/US/cp/10005001/Apps/72a6ba17-2d4e-4a1c-bcfb-cdc5d4b32d0e
            //details app http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps/5653a21d-3739-4d12-ab82-b389b06ffae9?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&cf=99-1
            //reviews http://marketplaceedgeservice.windowsphone.com/v9/ratings/product/ab1b9a80-a8bd-41d0-b8b4-771d3b0ceaee/reviews?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&chunksize=10&skuId=5ec7c261-b5ba-4912-a579-46adb4c3d9b7&orderBy=mostHelpful
            //http://marketplaceedgeservice.windowsphone.com/v9/ratings/product/ab1b9a80-a8bd-41d0-b8b4-771d3b0ceaee/reviews?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&chunksize=10&skuId=5ec7c261-b5ba-4912-a579-46adb4c3d9b7&orderBy=latest
            //queries http://cdn.marketplaceedgeservice.windowsphone.com/v8/catalog/queries?os=8.10.14219.0&zLocale=EN-CA&cc=US&lang=en-US&prefix=filmit&chunksize=4&includeApplications=true&includeAlbums=fal
            //search http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&chunkSize=50&q=filmit&cf=99-1

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await StatusBar.GetForCurrentView().HideAsync();
            GetPinnedLists();
            GetRecentLists();
        }

        private async void GetPinnedLists()
        {
            
          //  PinnedSection.DataContext =await LocalCacheHelper.OpenPinned();
        }

        private async void GetRecentLists()
        {
            await LocalCacheHelper.OpenRecent();
            //RecentSection.DataContext = list;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null) frame.Navigate(typeof(SearchPage));
        }

        private void RecentLst_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var entry = e.ClickedItem as Entry;
            LocalCacheHelper.AddRecentApp(entry);
            var frame = Window.Current.Content as Frame;
            if (frame != null && entry != null) frame.Navigate(typeof(AppDetailsPage), entry.Id);
        }
    }
}
