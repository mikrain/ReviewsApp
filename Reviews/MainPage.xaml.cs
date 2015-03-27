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
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;


            //icon http://cdn.marketplaceimages.windowsphone.com/v8/images/2f9c349d-e43e-42ae-a890-137154dddf3a?imageType=ws_icon_large
            //another details https://services.apps.microsoft.com/browse/6.2.9200-1/615/en-US_en-US/c/US/cp/10005001/Apps/72a6ba17-2d4e-4a1c-bcfb-cdc5d4b32d0e
            //details app http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps/5653a21d-3739-4d12-ab82-b389b06ffae9?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&cf=99-1
            //reviews https://services.apps.microsoft.com/4R/6.2.9200-1/1/en-US/m/US/Apps/f514d64b-8705-43b7-a400-c4f4f3dedfc0/Reviews/all/s/date/1/pn/1
            //queries http://cdn.marketplaceedgeservice.windowsphone.com/v8/catalog/queries?os=8.10.14219.0&zLocale=EN-CA&cc=US&lang=en-US&prefix=filmit&chunksize=4&includeApplications=true&includeAlbums=fal
            //search http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&chunkSize=50&q=filmit&cf=99-1
            var arrCounntryCodes = new[] { "AE", "AR", "AT", "AU", "BE", "BG", "BH", "CA", "CH", "CL", "CN", "CO", "CR", "CY", "CZ", "DE", "DK", "DZ", "EE", "EG", "ES", "FI", "FR", "GB", "GR", "HK", "HR", "HU", "ID", "IE", "IL", "IN", "IQ", "IT", "JO", "JP", "KW", "KZ", "LB", "LK", "LT", "LU", "LV", "LY", "MA", "MT", "MX", "MY", "NL", "NO", "NZ", "OM", "PE", "PH", "PK", "PL", "QA", "RO", "RS", "RU", "SA", "SE", "SG", "SI", "SK", "TH", "TN", "TR", "TT", "UA", "US", "UY", "VE", "VN" };

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
                        "http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&chunkSize=50&q=filmit&cf=99-1");
            var doc = XDocument.Parse(asf);
            XmlSerializer serializer = new XmlSerializer(typeof(SearchResult));
            var feed = (SearchResult)serializer.Deserialize(doc.CreateReader());
            await GetAppDetails(feed.Entry.Id);
        }

        private async Task GetAppDetails(string appId)
        {
            HttpClient client = new HttpClient();
            var asf = await client.GetStringAsync(
                        string.Format("http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps/{0}?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&cf=99-1",appId));
            var doc = XDocument.Parse(asf);
            //XmlSerializer serializer = new XmlSerializer(typeof(SearchResult));
            //var feed = (SearchResult)serializer.Deserialize(doc.CreateReader());
        }
    }
}
