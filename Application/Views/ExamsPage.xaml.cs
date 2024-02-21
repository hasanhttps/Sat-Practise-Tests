using System;
using Application.ViewModels;
using System.Windows.Controls;

namespace Application.Views;

public partial class ExamsPage : Page {
    public ExamsPage(Frame frame) {
        InitializeComponent();
        DataContext = new ExamPageModel(frame);
    }
}