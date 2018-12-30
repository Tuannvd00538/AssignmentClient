using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AssignmentClient.Frames;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AssignmentClient.Entity;

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
        private static string API_LOGIN = "https://oauth2servercontext.azurewebsites.net/_api/v1/Authentication";

        private async void Login_Button(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> LoginInform = new Dictionary<string, string>();
            LoginInform.Add("email", "vuongnd@gmail.com");
            LoginInform.Add("password", "vuong");
            HttpClient httpClient = new HttpClient();
            var response = new StringContent(JsonConvert.SerializeObject(LoginInform), System.Text.Encoding.UTF8, "application/json");
            var content = await httpClient.PostAsync(API_LOGIN, response).Result.Content.ReadAsStringAsync();
            Debug.WriteLine(content);

            if (this.Email.Text == "")
            {
                this.error_UserName.Text = "Please enter introduction";
            }
            if (this.Password.Password == "")
            {
                this.error_Password.Text = "Please enter introduction";
            }
        }

        private async void Page_loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("config_login.json");
            //    string info = await FileIO.ReadTextAsync(config_login);

            //    if (info != "")
            //    {

            //        this.Frame.Navigate(typeof(HomePage));
            //    }
            //}
            //catch (FileNotFoundException)
            //{
            //    this.Frame.Navigate(typeof(HomePage));
            //    Debug.WriteLine("login...");
            //}
        }
    }
}
