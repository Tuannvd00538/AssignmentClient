using AssignmentClient.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AssignmentClient.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page
    {
        
        private ObservableCollection<Classes> ObservableClasses = new ObservableCollection<Classes>();

        public Dashboard()
        {

            this.InitializeComponent();

        }

        private void ShowClass(object sender, ItemClickEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ShowClass));
        }

        private static string GetListClass = "https://backendcontroller.azurewebsites.net/_api/v1/HandleClientAPI/Class/";

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String credential = await FileIO.ReadTextAsync(file);

            JsonObject data = JsonObject.Parse(credential);

            HttpClient client = new HttpClient();
            var response = httpClient.GetAsync(GetListClass + data.GetNamedValue("ownerId"));
            var res = await response.Result.Content.ReadAsStringAsync();

            List<Classes> classes = JsonConvert.DeserializeObject<List<Classes>>(res);



            foreach (var item in classes)
            {
                ObservableClasses.Add(item);
                
            }
        }
        private void CommandInvokedHandler(IUICommand command)
        {

        }
        private async void Mess(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Chức năng đang phát triển.");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Đóng",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}
