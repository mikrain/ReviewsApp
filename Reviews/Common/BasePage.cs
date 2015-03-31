using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CinelabWP8_1.Common;

namespace Reviews.Common
{
    public abstract class BasePage : Page
    {
        protected readonly NavigationHelper navigationHelper;

        protected BasePage()
        {
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            Application.Current.Resuming -= new EventHandler<Object>(App_Resuming);
            this.navigationHelper.LoadState -= this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState -= this.NavigationHelper_SaveState;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (ChildAllowedToExit(e))
            {
                e.Handled = true;
            }
        }

        private void App_Resuming(object sender, object e)
        {
           
        }

        protected abstract void NavigationHelper_LoadState(object sender, LoadStateEventArgs e);
        protected abstract void NavigationHelper_SaveState(object sender, SaveStateEventArgs e);
        protected abstract bool ChildAllowedToExit(BackPressedEventArgs e);
    }
}
