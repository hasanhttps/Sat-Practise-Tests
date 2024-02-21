using System;
using System.Windows;
using Application.ViewModels;
using System.Windows.Controls;
using Database.Entities.Concretes;

namespace Application.Views {
    public partial class QuizPage : Page {

        // Constructor

        public QuizPage(Frame frame, Exam exam) {
            InitializeComponent();
            MakeWindowFullscreen();
            DataContext = new QuizPageModel(frame, exam, AnswersLV, OpenEndedAnswerTB);
        }

        // Functions

        private void MakeWindowFullscreen() {
            Window window = System.Windows.Application.Current.MainWindow;
            if (window != null) {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;

                window.Visibility = Visibility.Collapsed;
                window.Topmost = true;
                window.Visibility = Visibility.Visible;
            }
        }
    }
}
