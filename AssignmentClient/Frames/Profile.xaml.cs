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
    public sealed partial class Profile : Page
    {
        private static string GetProfile_Api = "https://backendcontroller.azurewebsites.net/_api/v1/Accounts/";
        public Profile()
        {
            this.InitializeComponent();
        }
        public ImageSource ProfilePicture { get; set; }
        private void Edit_Profile(object sender, TappedRoutedEventArgs e)
        {
            //var rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate(typeof(Frames.EditProfile), null, new DrillInNavigationTransitionInfo());
            
        }

        private async void GetProfile(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("info.txt");
            String credential = await FileIO.ReadTextAsync(file);

            dynamic profile = JsonConvert.DeserializeObject(credential);
            
            this.FullName.Text = profile.FirstName + " " + profile.LastName;
            this.RollNumber.Text = profile.RollNumber.ToString().ToUpper();
            this.Email.Text = "Email: " + profile.Email;
            this.rollNumber.Text = "Mã Sinh Viên: " + profile.RollNumber.ToString().ToUpper();
            this.fullName.Text = "Họ Tên: " + profile.FirstName + " " + profile.LastName;
            this.Phone.Text = "Số Điện Thoại: " + profile.Phone;
            this.Birthday.Text = "Ngày Sinh: " + profile.BirthDay;
            var gender = (int)profile.Gender;
            var gioitinh = "";
            if (gender == 0)
            {
                gioitinh = "Nam";
            } else if (gender == 1)
            {
                gioitinh = "Nữ";
            } else if (gender == 2)
            {
                gioitinh = "Khác";
            }
            this.Gender.Text = "Gender: " + gioitinh;
            this.AvatarApp.ImageSource = new BitmapImage(new Uri(profile.Avatar.ToString()));
            Debug.WriteLine(credential);
        }
    }
}
