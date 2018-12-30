using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AssignmentClient.Frames;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AssignmentClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Login_Button(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Xử lý login ở đây");
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("config_login.json");
                String info = await FileIO.ReadTextAsync(config_login);

                if (info != "")
                {

                    this.Frame.Navigate(typeof(HomePage));
                }
            }
            catch (FileNotFoundException)
            {
                this.Frame.Navigate(typeof(HomePage));
                Debug.WriteLine("Login...");
            }
        }
    }
}
