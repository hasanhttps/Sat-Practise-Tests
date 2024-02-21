using System;
using System.Linq;
using Application.Views;
using System.Windows.Controls;

namespace Application.ViewModels;

public class MainViewModel {

    // Private Fields

    private Frame MainFrame;

    // Constructor

    public MainViewModel(Frame frame) {

        MainFrame = frame;
        MainFrame.Content = new LoginPage(MainFrame);
    }

    // Functions


}
