using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public EditProfile()
        {
            this.InitializeComponent();
            this.Email.IsEnabled = false;
            this.Email.Text = "abc.xzy.com";
            this.RollNumber.IsEnabled = false;
        }

        private void Edit_Save(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Frames.Profile));

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
                //string postData = Convert.ToBase64String(file);
                //byte[] bytes = enc.GetBytes(postData);

                byte[] bytes = buffer.ToArray();

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                Stream writer = request.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);

                var response = (HttpWebResponse)request.GetResponse();
                var url = new StreamReader(response.GetResponseStream());
                JObject objImgur = JObject.Parse(url.ReadToEnd());

                Debug.WriteLine(objImgur);

                //account.avatar = objImgur.SelectToken("data").SelectToken("link").ToString();

                YourAvatar.ProfilePicture = bitmapImage;
            }

        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
        }
    }
}
