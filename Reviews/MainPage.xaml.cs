using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel.Store;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Reviews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly string[] _arrCounntryCodes = new[] { "AE", "AR", "AT", "AU", "BE", "BG", "BH", "CA", "CH", "CL", "CN", "CO", "CR", "CY", "CZ", "DE", "DK", "DZ", "EE", "EG", "ES", "FI", "FR", "GB", "GR", "HK", "HR", "HU", "ID", "IE", "IL", "IN", "IQ", "IT", "JO", "JP", "KW", "KZ", "LB", "LK", "LT", "LU", "LV", "LY", "MA", "MT", "MX", "MY", "NL", "NO", "NZ", "OM", "PE", "PH", "PK", "PL", "QA", "RO", "RS", "RU", "SA", "SE", "SG", "SI", "SK", "TH", "TN", "TR", "TT", "UA", "US", "UY", "VE", "VN" };


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
            await SearchResult();
        }

        private async Task SearchResult()
        {
            HttpClient client = new HttpClient();
            var asf =
                await
                    client.GetStringAsync(
                        "http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&chunkSize=50&q=facebook&cf=99-1");
            var doc = XDocument.Parse(asf);
            XmlSerializer serializer = new XmlSerializer(typeof(SearchResult));
            var feed = (SearchResult)serializer.Deserialize(doc.CreateReader());
            await GetAppDetails(feed.Entry.Id);

        }

        private async Task GetReviews(string appId, string skuId)
        {
            List<ReviewEntry> comments = new List<ReviewEntry>();
            HttpClient client = new HttpClient();
            XmlSerializer serializer = new XmlSerializer(typeof(ReviewsDetails));
            List<Task> tasks = new List<Task>();

            foreach (var arrCounntryCode in _arrCounntryCodes)
            {
                tasks.Add(Task.Run(async () =>
                  {
                      var asf = await client.GetStringAsync(
                             string.Format("http://marketplaceedgeservice.windowsphone.com/v9/ratings/product/{0}/reviews?os={3}&cc={1}&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&chunksize=10&skuId={2}&orderBy=latest", appId, arrCounntryCode, skuId, "8.10.14219.0"));
                      if (!string.IsNullOrEmpty(asf))
                      {
                          var doc = XDocument.Parse(asf);
                          var review = (ReviewsDetails)serializer.Deserialize(doc.CreateReader());
                          lock (comments)
                          {
                              comments.AddRange(review.Entry);
                          }
                      }
                  }));
            }

            await Task.WhenAll(tasks);
            lstComments.ItemsSource = comments.OrderBy(entry => entry.Updated);
        }

        private async Task GetAppDetails(string appId)
        {
            HttpClient client = new HttpClient();
            var asf = await client.GetStringAsync(
                        string.Format("http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps/{0}?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&cf=99-1", appId));
            var doc = XDocument.Parse(asf);
            XmlSerializer serializer = new XmlSerializer(typeof(AppDetails));
            var feed = (AppDetails)serializer.Deserialize(doc.CreateReader());


            await GetReviews(appId, feed.Entry.SkuId);
        }
    }
}
