using System;
using System.Windows;

namespace Application.Views {
    public partial class Desmos : Window {
        public Desmos(string url) {
            InitializeComponent();
            Loaded += async (_, _) => {
                await WebView.EnsureCoreWebView2Async(); // Initialize WebView2
                WebView.Source = new Uri(url);          // Navigate to the URL
            };
        }
    }
}
