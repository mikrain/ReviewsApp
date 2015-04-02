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
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
    public sealed partial class AppDetailsPage : BasePage
    {
        readonly string[] _arrCounntryCodes = new[] { "AE", "AR", "AT", "AU", "BE", "BG", "BH", "CA", "CH", "CL", "CN", "CO", "CR", "CY", "CZ", "DE", "DK", "DZ", "EE", "EG", "ES", "FI", "FR", "GB", "GR", "HK", "HR", "HU", "ID", "IE", "IL", "IN", "IQ", "IT", "JO", "JP", "KW", "KZ", "LB", "LK", "LT", "LU", "LV", "LY", "MA", "MT", "MX", "MY", "NL", "NO", "NZ", "OM", "PE", "PH", "PK", "PL", "QA", "RO", "RS", "RU", "SA", "SE", "SG", "SI", "SK", "TH", "TN", "TR", "TT", "UA", "US", "UY", "VE", "VN" };


        public AppDetailsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await StatusBar.GetForCurrentView().HideAsync();
            if (e.Parameter != null) GetAppDetails(e.Parameter.ToString());
        }

        private void GetAppDetails(string appId)
        {
            Task.Run(async () =>
               {
                   try
                   {
                       HttpClient client = new HttpClient();
                       var asf = await client.GetStringAsync(
                                   string.Format("http://marketplaceedgeservice.windowsphone.com/v9/catalog/apps/{0}?os=8.10.14219.0&cc=US&lang=en-US&hw=520190979&dm=RM-821_apac_hong_kong_234&oemId=NOKIA&moId=&cf=99-1", appId));
                       var doc = XDocument.Parse(asf);
                       XmlSerializer serializer = new XmlSerializer(typeof(AppDetails));
                       var feed = (AppDetails)serializer.Deserialize(doc.CreateReader());
                       Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                       {
                           CheckPin(feed.Title);
                           DetailsPivot.DataContext = feed;
                           rootPivot.Title = feed.Title;
                       });
                       GetReviews(appId, feed.Entry.SkuId);
                   }
                   catch (Exception)
                   {

                   }

               });
        }

        private void CheckPin(string title)
        {
            if (LocalCacheHelper.PinnedApps.FirstOrDefault(entry => entry.Title == title) == null)
            {
                PinPanel.Visibility = Visibility.Visible;
                UnpinPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                UnpinPanel.Visibility = Visibility.Visible;
                PinPanel.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetReviews(string appId, string skuId)
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

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                prgBar.Visibility = Visibility.Collapsed;
                lstComments.DataContext = comments.OrderByDescending(entry => entry.Updated);
            });

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

        private void pinClick(object sender, RoutedEventArgs e)
        {
            var entry1Context = DetailsPivot.DataContext as AppDetails;
            LocalCacheHelper.AddPinnedApp(new Entry() { Id = entry1Context.Id, Image = entry1Context.Image, Title = entry1Context.Title });
            CheckPin(entry1Context.Title);
        }

        private void UnPInClick(object sender, RoutedEventArgs e)
        {
            var entry1Context = DetailsPivot.DataContext as AppDetails;
            var entryToRemove = LocalCacheHelper.PinnedApps.FirstOrDefault(entry => entry.Title == entry1Context.Title);
            LocalCacheHelper.RemovePinnedApp(entryToRemove);
            CheckPin(entryToRemove.Title);
        }
    }
}
