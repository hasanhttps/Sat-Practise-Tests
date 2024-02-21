using System;
using Application.ViewModels;
using System.Windows.Controls;

namespace Application.Views {
    public partial class LoginPage : Page {
        public LoginPage(Frame frame) {
            InitializeComponent();
            DataContext = new LoginPageModel(frame);
        }
    }
}
