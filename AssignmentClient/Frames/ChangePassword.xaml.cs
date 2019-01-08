using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AssignmentClient.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePassword : Page
    {
        private static string API_ChangePassword = "https://backendcontroller.azurewebsites.net/_api/v1/Authentication/ChangePassword";
        public ChangePassword()
        {
            this.InitializeComponent();
        }

        private bool Validate()
        {
            bool result;
            if (this.Old_Password.Password.ToString() == "")
            {
                result = false;
                this.error_OldPassword.Text = "Vui Lòng Điền Thông Tin!";
            }
            else
            {
                result = true;
                this.error_OldPassword.Visibility = Visibility.Collapsed;
            }
            if (this.New_Password.Password.ToString() == "")
            {
                result = false;
                this.error_NewPassword.Text = "Vui Lòng Điền Thông Tin!";
            }
            else
            {
                result = true;
                this.error_NewPassword.Visibility = Visibility.Collapsed;
            }
            if (this.Re_Password.Password.ToString() == "")
            {
                result = false;
                this.error_RePassword.Text = "Bạn chưa nhập lại mật khẩu!";
            }
            else if (this.New_Password.Password.ToString() != this.Re_Password.Password.ToString())
            {
                result = false;
                this.error_RePassword.Text = "Mật khẩu không khớp!";
            }
            else
            {
                result = true;
                this.error_RePassword.Visibility = Visibility.Collapsed;
            }
            return result;
        }
        private void CommandInvokedHandler(IUICommand command)
        {

        }
        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                // Create the message dialog and set its content
                var messageDialog = new MessageDialog("Bạn có chắc muốn dùng mật khẩu này?");

                // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                messageDialog.Commands.Add(new UICommand(
                    "Có",
                    new UICommandInvokedHandler(ChangePasswords)));
                messageDialog.Commands.Add(new UICommand(
                    "Không",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;

                // Show the message dialog
                await messageDialog.ShowAsync();

            }
        }

        private async void ChangePasswords(IUICommand command)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String credential = await FileIO.ReadTextAsync(file);
            dynamic profile = JsonConvert.DeserializeObject(credential);

            Debug.WriteLine(credential);

            Dictionary<String, String> ChangePassword = new Dictionary<string, string>
                {
                    { "OwnerId", profile.ownerId.ToString() },
                    { "OldPassword", this.Old_Password.Password.ToString() },
                    { "NewPassword", this.New_Password.Password.ToString() }
                };

            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(ChangePassword), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_ChangePassword, content).Result;
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this.QuayQuay.IsActive = true;
                var messageDialog = new MessageDialog("Thay đổi mật khẩu thành công");

                // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                messageDialog.Commands.Add(new UICommand(
                    "Ok",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;

                // Show the message dialog
                await messageDialog.ShowAsync();
                this.Frame.Navigate(typeof(Frames.Profile));
            }
            else
            {
                Debug.WriteLine("Debug Error:" + responseContent);
                this.error_RePassword.Text = "Password is incorrect";
                this.error_RePassword.Visibility = Visibility.Visible;
            }
        }
    }
}
