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
        private static string API_LOGIN = "https://oauth2servercontext.azurewebsites.net/_api/v1/Authentication/Login";

        private async void Login_Button(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> LoginInfo = new Dictionary<string, string>();
            LoginInfo.Add("email", this.Email.Text);
            LoginInfo.Add("password", this.password.Text);

            
       
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(LoginInfo), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_LOGIN, content).Result;
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // save file...
                Debug.WriteLine("Debug Success:" + responseContent);
                // Doc token
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                // Luu token
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, responseContent);

                // Lay thong tin ca nhan bang token.
                HttpClient httpClient2 = new HttpClient();
                var response2 = new StringContent(JsonConvert.SerializeObject(LoginInfo), System.Text.Encoding.UTF8, "application/json");
                var content2 = await httpClient.PostAsync(API_LOGIN, response2).Result.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                this.Frame.Navigate(typeof(HomePage));
            }
            else
            {
                Debug.WriteLine("Debug Error:" + responseContent);
               // //Xu ly loi.
               //ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
               // if (errorObject != null)
               // {
               //     this.error_UserName.Text = "username or password is incorrect";
               //     this.error_UserName.Visibility = Visibility.Visible;
               // }
            }


            if (this.Email.Text == "")
            {
                this.error_UserName.Text = "Please enter introduction";
            }
            if (this.Password.Password == "")
            {
                this.error_Password.Text = "Please enter introduction";
            }
        }

        private void Page_loaded(object sender, RoutedEventArgs e)
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
