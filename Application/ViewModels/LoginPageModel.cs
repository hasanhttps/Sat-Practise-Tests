using System;
using System.Linq;
using System.Windows;
using System.Threading;
using Application.Views;
using Database.DbContexts;
using Application.Commands;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using static Application.Models.DatabaseNamespace.Database;

namespace Application.ViewModels;

public class LoginPageModel : INotifyPropertyChanged {

    // Private Fields

    private Frame MainFrame;
    private int? _schoolnumber;
    private Thread loadDatabase;

    // Binding Properties

    public int? SchoolNumber { get => _schoolnumber;
        set { 
            _schoolnumber = value;
            OnProperty();
        }
    }
    public ICommand? LogInButtonCommand { get; set; }

    // Constructor

    public LoginPageModel(Frame frame) { 

        MainFrame = frame;
        SetCommands();

        loadDatabase = new Thread(() => {
            DbContext = new SatExaminationDbContext();
            SatStudents = DbContext.SatStudents.ToList();
        });
        loadDatabase.Start();
    }

    // Functions

    private void LogIn(object? param) {

        if (SchoolNumber != null) {
            loadDatabase.Join();

            bool isContain = false;
            foreach(var student in SatStudents) {
                if (student.SchoolNumber == SchoolNumber) {
                    CurrentStudent = student;
                    isContain = true;
                }
            }

            if (isContain) MainFrame.Content = new ExamsPage(MainFrame);
            else MessageBox.Show("School number is not valid. Please try again, if error present let us the problem.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else MessageBox.Show("Please enter your school number. If error present let us the problem.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void SetCommands() {

        LogInButtonCommand = new RelayCommand(LogIn);
    }

    // INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnProperty([CallerMemberName] string? name = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
