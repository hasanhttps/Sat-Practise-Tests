using System;
using System.Linq;
using System.Windows;
using System.Threading;
using Application.Views;
using Database.DbContexts;
using Application.Commands;
using System.Windows.Input;
using System.Windows.Controls;
using Database.Entities.Concretes;
using System.Collections.ObjectModel;
using static Application.Models.DatabaseNamespace.Database;

namespace Application.ViewModels;

public class ExamPageModel {

    // Private Fields

    private Frame MainFrame;

    // Binding Properties

    public ObservableCollection<Exam> Exams { get; set; } = new();
    public ICommand SelectionChangedCommand { get; set; }

    // Constructor

    public ExamPageModel(Frame frame) { 
        
        MainFrame = frame;

        SelectionChangedCommand = new RelayCommand(SelectionChanged);

        foreach (var exam in DbContext.Exams.ToList()) {
            Exams.Add(exam);
        }
    }

    // Functions
    
    private void SelectionChanged(object? param) {
        Exam? exam = (param as Exam);
        bool isError = false;

        if (exam != null) {

            if (DateTime.Now < exam.ActivationDate) {
                MessageBox.Show("Exam is not activated yet. Please wait for activation date and time of exam", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                isError = true;
            }

            if (exam.Modules.ToList() == null) {
                MessageBox.Show("In this exam there are no modules. Please notify us.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                isError = true;
            }

            if (exam.Modules.Count != 4) {
                MessageBox.Show("In this exam modules are not completed. Please notify us.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                isError = true;
            }

            if (isError) MainFrame.Content = new ExamsPage(MainFrame);
            else MainFrame.Content = new QuizPage(MainFrame, exam);
        }
    }
}