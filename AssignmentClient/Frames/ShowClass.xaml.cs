using AssignmentClient.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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
    public sealed partial class ShowClass : Page
    {
        private List<Classes> Classes;
        private List<Subjects> Subjects;

        public ShowClass()
        {
            this.InitializeComponent();
            this.ViewModel = new SubjectViewModel();
            //Subjects = SubjectsManager.GetSubject();

            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }
        public SubjectViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
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

        private void LoadMark(object sender, ItemClickEventArgs e)
        {
            this.FindName("ShowMarks");
            this.FindName("Marks");
        }

        private void ListSubject(object sender, ItemClickEventArgs e)
        {
            this.FindName("LoadListSubject");
            this.FindName("LoadSubjects");
        }

        private static string GetListClass = "https://backendcontroller.azurewebsites.net/_api/v1/HandleClientAPI/Class/";
        private ObservableCollection<Classes> ObservableClasses = new ObservableCollection<Classes>();

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
    }
}
