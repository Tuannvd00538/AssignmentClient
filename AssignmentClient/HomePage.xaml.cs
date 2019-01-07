using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AssignmentClient.Frames;
using Windows.UI.Xaml.Media.Animation;
using Windows.Storage;
using System.Net.Http;
using Windows.Data.Json;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.UI.Popups;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AssignmentClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private static string GetProfile_Api = "https://backendcontroller.azurewebsites.net/_api/v1/Accounts/";

        public HomePage()
        {
            this.InitializeComponent();
        }

        // Add "using" for WinUI controls.
        // using muxc = Microsoft.UI.Xaml.Controls;

        private Type currentPage;

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page 
        private readonly IList<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("report", typeof(Dashboard)),
            ("profile", typeof(Profile)),
            ("redirect", typeof(Profile)),
            ("changePassword", typeof(ChangePassword))
        };

        private async void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // You can also add items in code behind
            NavView.MenuItems.Add(new NavigationViewItemSeparator());

            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default: you need to specify it
            NavView_Navigate("report");

            // Add keyboard accelerators for backwards navigation
            var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);


            //Load profile
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String credential = await FileIO.ReadTextAsync(file);

            JsonObject data = JsonObject.Parse(credential);

            HttpClient client = new HttpClient();
            var response = httpClient.GetAsync(GetProfile_Api + data.GetNamedValue("ownerId"));
            var res = await response.Result.Content.ReadAsStringAsync();

            StorageFile info = await folder.CreateFileAsync("info.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(info, res);

            dynamic profile = JsonConvert.DeserializeObject(res);

            this.AvatarApp.ImageSource = new BitmapImage(new Uri(profile.Avatar.ToString()));
            this.FullName.Text = profile.FirstName + " " + profile.LastName;
            this.RollNumber.Text = profile.RollNumber.ToString().ToUpper();
        }
        private async void handlerLogoutAsync(IUICommand command)
        {
            StorageFile deleteCurrentToken = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            StorageFile deleteCurrentInfo = await ApplicationData.Current.LocalFolder.GetFileAsync("info.txt");

            await deleteCurrentToken.DeleteAsync();
            await deleteCurrentInfo.DeleteAsync();
            this.Frame.Navigate(typeof(MainPage));

        }
        private void CommandInvokedHandler(IUICommand command)
        {

        }
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem == null)
                return;

            if (args.IsSettingsInvoked)
                ContentFrame.Navigate(typeof(HomePage));
            else
            {
                // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
                var navItemTag = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(i => args.InvokedItem.Equals(i.Content))
                    .Tag.ToString();

                NavView_Navigate(navItemTag);
            }
        }

        private void NavView_Navigate(string navItemTag)
        {
            var item = _pages.First(p => p.Tag.Equals(navItemTag));
            if (currentPage == item.Page)
                return;
            ContentFrame.Navigate(item.Page);

            currentPage = item.Page;
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => On_BackRequested();

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(HomePage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            }
            else
            {
                var item = _pages.First(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
            }
        }

        private async void logoutClick(object sender, TappedRoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Bạn có chắc chắn muốn đăng xuất?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Hủy",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "Đăng xuất",
                new UICommandInvokedHandler(handlerLogoutAsync)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}
