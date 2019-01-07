using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public ChangePassword()
        {
            this.InitializeComponent();
        }

        private void Validate()
        {
            if (this.Old_Password.Password.ToString() == "")
            {
                this.error_OldPassword.Text = "Vui Lòng Điền Thông Tin!";
            }
            else
            {
                this.error_OldPassword.Visibility = Visibility.Collapsed;
            }
            if (this.New_Password.Password.ToString() == "")
            {
                this.error_NewPassword.Text = "Vui Lòng Điền Thông Tin!";
            }
            else
            {
                this.error_NewPassword.Visibility = Visibility.Collapsed;
            }
            if (this.Re_Password.Password.ToString() == "")
            {
                this.error_RePassword.Text = "Bạn chưa nhập lại mật khẩu!";
            }
            else if (this.New_Password.Password.ToString() != this.Re_Password.Password.ToString())
            {
                this.error_RePassword.Text = "Mật khẩu không khớp!";
            }
            else
            {
                this.error_RePassword.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Validate();
        }
    }
}
