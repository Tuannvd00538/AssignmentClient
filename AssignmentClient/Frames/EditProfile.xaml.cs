using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AssignmentClient.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditProfile : Page
    {
        private static string EditProfile_Api = "https://backendcontroller.azurewebsites.net/_api/v1/Accounts/";

        private int gender;
        public EditProfile()
        {
            this.InitializeComponent();
            this.Email.IsEnabled = false;
            this.Email.Text = "";
            this.RollNumber.IsEnabled = false;
            GetProfile();
            
        }

        private async void GetProfile()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("info.txt");
            String credential = await FileIO.ReadTextAsync(file);

            dynamic profile = JsonConvert.DeserializeObject(credential);

            FirstName.Text = profile.FirstName;
            LastName.Text = profile.LastName;
            RollNumber.Text = profile.RollNumber.ToString().ToUpper();
            Email.Text =  profile.Email;
            RollNumber.Text =  profile.RollNumber.ToString().ToUpper();
            Phone.Text = profile.Phone;
            BirthDay.Date = profile.BirthDay;
            Avatar.ImageSource = new BitmapImage(new Uri(profile.Avatar.ToString()));
            AvatarUrl.Text = profile.Avatar;
        }

        private void FrameBack(IUICommand command)
        {
            On_BackRequested();
        }
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private async void Edit_Save(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                Dictionary<String, String> Edit_Info = new Dictionary<string, string>
                {
                    {"FirstName", this.FirstName.Text},
                    {"LastName", this.LastName.Text},
                    {"Phone", this.Phone.Text},
                    {"Birthday", this.BirthDay.Date.ToString()},
                    {"Avatar", this.AvatarUrl.Text},
                    {"Gender", gender.ToString()},
                };

                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
                String credential = await FileIO.ReadTextAsync(file);

                JsonObject data = JsonObject.Parse(credential);

                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(Edit_Info), System.Text.Encoding.UTF8, "application/json");
                var response = httpClient.PutAsync(EditProfile_Api + data.GetNamedValue("ownerId"), content).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this.QuayQuay.IsActive = true;
                    StorageFile info = await ApplicationData.Current.LocalFolder.GetFileAsync("info.txt");
                    await FileIO.WriteTextAsync(info, responseContent);


                    // Create the message dialog and set its content
                    var messageDialog = new MessageDialog("Sửa thông tin thành công!");

                    // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                    messageDialog.Commands.Add(new UICommand(
                        "OK",
                        new UICommandInvokedHandler(FrameBack)));

                    // Set the command that will be invoked by default
                    messageDialog.DefaultCommandIndex = 0;

                    // Set the command to be invoked when escape is pressed
                    messageDialog.CancelCommandIndex = 1;

                    // Show the message dialog
                    await messageDialog.ShowAsync();
                    //this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    Debug.WriteLine("Debug Error:" + responseContent);
                    //this.error_Password.Text = "username or password is incorrect";
                    // //Xu ly loi.
                    //ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                    // if (errorObject != null)
                    // {

                    // }
                }
                
            }
            
        }

        private async void Select_Photo(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,

                ViewMode = PickerViewMode.Thumbnail
            };

            fileOpenPicker.FileTypeFilter.Clear();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");

            StorageFile fileLocal = await fileOpenPicker.PickSingleFileAsync();
            if (fileLocal != null)
            {
                IRandomAccessStream fileStream =
                await fileLocal.OpenAsync(FileAccessMode.Read);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);

                IBuffer buffer = await FileIO.ReadBufferAsync(fileLocal);

                var request = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/image");
                request.Headers.Add("Authorization", "Client-ID 490b09a77765fd3");
                request.Method = "POST";

                ASCIIEncoding enc = new ASCIIEncoding();

                byte[] bytes = buffer.ToArray();

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                Stream writer = request.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);

                var response = (HttpWebResponse)request.GetResponse();
                var url = new StreamReader(response.GetResponseStream());
                JObject objImgur = JObject.Parse(url.ReadToEnd());

                Avatar.ImageSource = bitmapImage;
                AvatarUrl.Text = objImgur.SelectToken("data").SelectToken("link").ToString();
            }

        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            gender = Int32.Parse(radioGender.Tag.ToString());
        }

        private void Cancle(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
            
        }

        private bool validate()
        {
            bool result;
            if (this.FirstName.Text == "" && this.Phone.Text == "" && this.LastName.Text == "")
            {
                result = false;
                this.error_Input.Text = "Vui Lòng Điền Đầy Đủ Thông Tin!";
            }
            else
            {
                result = true;
                this.error_Input.Visibility = Visibility.Collapsed;
            }
            return result;
        }
    }

}
