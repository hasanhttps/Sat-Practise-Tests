using System;
using System.IO;
using System.Linq;
using System.Windows;
using Application.Views;
using System.Windows.Input;
using Application.Commands;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using Database.Entities.Concretes;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using static Application.Models.DatabaseNamespace.Database;

namespace Application.ViewModels;

public class QuizPageModel : INotifyPropertyChanged {

    // Private Fields

    private Module module;
    private Frame MainFrame;
    private Exam CurrentExam;
    private ListView AnswersLV;
    private int questionIndex = 0;
    private DispatcherTimer timer;
    private TimeSpan remainingTime;
    private string remainedTimeTxt;
    private Question currentQuestion;
    private TextBox OpenEndedAnswerTB;
    private BitmapImage questionImage;
    private Dictionary<Question, Answer> KeyValuePairs = new();

    // Binding Properties

    public ICommand NextButtonCommand { get; set; }
    public ICommand PreviousButtonCommand { get; set; }
    public ICommand SelectionChangedCommand { get; set; }
    public string RemainingTimeTxt { get => remainedTimeTxt;
        set {
            remainedTimeTxt = value;
            OnProperty();
        }
    }
    public Question CurrentQuestion { get => currentQuestion; 
        set { 
            currentQuestion = value;
            OnProperty();
        }
    }

    public BitmapImage QuestionImage { get => questionImage;
        set {
            questionImage = value;
            OnProperty();
        }
    }

    public Module Module { get => module; 
        set { 
            module = value;
            OnProperty();
        }
    }

    // Constructor

    public QuizPageModel(Frame frame, Exam exam, ListView answers, TextBox openEndedAnswerBox) { 
        
        MainFrame = frame;
        CurrentExam = exam;
        AnswersLV = answers;
        OpenEndedAnswerTB = openEndedAnswerBox;

        NextButtonCommand = new RelayCommand(NextQuestion);
        PreviousButtonCommand = new RelayCommand(PreviousQuestion);
        SelectionChangedCommand = new RelayCommand(SelectionChanged);

        StartExam();
    }

    // Functions


    private void PreviousQuestion(object? param) {
        if (questionIndex == 0) return;
        questionIndex--;
        SetQuestions();
    }

    private void NextQuestion(object? param) {
        IfOpenEndedSetAnswer();
        if (questionIndex + 1 == Module.Questions.Count) return;
        questionIndex++;
        SetQuestions();
    }

    private void StartTimer() {
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void IfOpenEndedSetAnswer() {

        if(currentQuestion.isOpenEnded) {

            Answer answer = new Answer() {
                AnswerText = OpenEndedAnswerTB.Text,
                isOpenEnded = true,
                isCorrect = (OpenEndedAnswerTB.Text == currentQuestion.Answers.ToList()[0].AnswerText) ? true : false
            };

            if (KeyValuePairs.ContainsKey(CurrentQuestion) == true)
                KeyValuePairs[CurrentQuestion] = answer;
            else
                KeyValuePairs.Add(CurrentQuestion, answer);
        }
    }

    public void StartExam() {

        Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Verbal" && p.ModuleNumber == 1).First();
        SetQuestions();
        remainedTimeTxt = $"{Module.Time.ToString()}:00";
        remainingTime = TimeSpan.FromMinutes(Module.Time);
        StartTimer();
    }

    private void Timer_Tick(object? sender, EventArgs e) {

        remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
        RemainingTimeTxt = remainingTime.ToString(@"mm\:ss");

        if (remainingTime <= TimeSpan.Zero) {
            timer.Stop();
            SetNextModule();
        }
    }

    private void SelectionChanged(object? param) {
        Answer? answer = (param as Answer);
        if (answer != null) {
            if (KeyValuePairs.ContainsKey(CurrentQuestion) == true)
                KeyValuePairs[CurrentQuestion] = answer;
            else
                KeyValuePairs.Add(CurrentQuestion, answer);
        }
    }

    private void RestoreWindows() {
        Window window = System.Windows.Application.Current.MainWindow;
        if (window != null) {
            window.WindowStyle = WindowStyle.SingleBorderWindow;

            window.Visibility = Visibility.Collapsed;
            window.Topmost = false;
            window.Visibility = Visibility.Visible;
        }
    }

    public void SetQuestions() {

        CurrentQuestion = Module.Questions.ToList()[questionIndex];
        QuestionImage = LoadImage(CurrentQuestion.QuestionImage)!;

        if (CurrentQuestion.isOpenEnded) {
            AnswersLV.Visibility = Visibility.Collapsed;
            OpenEndedAnswerTB.Visibility = Visibility.Visible;
        }
        else {
            AnswersLV.Visibility = Visibility.Visible;
            OpenEndedAnswerTB.Visibility = Visibility.Collapsed;
        }
    }

    public void SetNextModule() {

        questionIndex = 0;
        IfOpenEndedSetAnswer();
        if (Module.Subject == "Sat Verbal" && Module.ModuleNumber == 1) Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Verbal" && p.ModuleNumber == 2).First();
        else if (Module.Subject == "Sat Verbal" && Module.ModuleNumber == 2) Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Math" && p.ModuleNumber == 1).First();
        else if (Module.Subject == "Sat Math" && Module.ModuleNumber == 1) Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Math" && p.ModuleNumber == 2).First();
        else if (Module.Subject == "Sat Math" && Module.ModuleNumber == 2) {
            StopExam();
            return;
        }
        else {
            MessageBox.Show("There is no more modules. This problem catched when teachers don't added properly Module or when system have problem. Please notify us.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SetQuestions();
        remainedTimeTxt = $"{Module.Time.ToString()}:00";
        remainingTime = TimeSpan.FromMinutes(Module.Time);
        StartTimer();
    }

    public void StopExam() {

        float mathpoint = 0, verbalpoint = 0;
        int correct = 0, questioncount = 0;

        foreach(var module in CurrentExam.Modules) {
            questioncount += module.Questions.Count;
        }

        StudentsExamResults studentsExamResults = new StudentsExamResults() {

            ExamId = CurrentExam.Id,
            StudentId = CurrentStudent.Id
        };
        DbContext.StudentsExamResults.Add(studentsExamResults);
        DbContext.SaveChanges();

        foreach (var pair in KeyValuePairs) {

            string studentanswer;
            if (!pair.Value.isOpenEnded) studentanswer = pair.Value.Variant;
            else studentanswer = pair.Value.AnswerText;

            string correctanswer = "N/A";
            foreach(var answer in pair.Key.Answers)
                if (answer.isCorrect) {
                    if (!answer.isOpenEnded) correctanswer = answer.Variant;
                    else correctanswer = pair.Value.AnswerText;
                    break;
                }


            StudentsAnswers studentsAnswers = new StudentsAnswers() {

                StudentAnswer = studentanswer,
                CorrectAnswer = correctanswer,
                isCorrect = (pair.Value.isCorrect) ? "+" : "-",
                QuestionNumber = pair.Key.QuestionNumber,
                Point = pair.Key.QuestionPoint,
                ModuleId = pair.Key.ModuleId,
                StudentsExamResultsId = studentsExamResults.Id
            };
            DbContext.StudentsAnswers.Add(studentsAnswers);
            DbContext.SaveChanges();


            if (pair.Value.isCorrect) {
                correct++;
                if (pair.Key.Module.Subject == "Sat Verbal")
                    verbalpoint += pair.Key.QuestionPoint;
                else if (pair.Key.Module.Subject == "Sat Math")
                    mathpoint += pair.Key.QuestionPoint;
            }
        }

        DbContext.ExamResults.Add(new ExamResult() {

            ExamId = CurrentExam.Id,
            StudentId = CurrentStudent.Id,
            MathScore = mathpoint,
            CorrectCount = correct,
            VerbalScore = verbalpoint,
            QuestionCount = questioncount,
            Score = mathpoint + verbalpoint
        });
        DbContext.SaveChanges();

        RestoreWindows();
        MainFrame.Content = new ExamsPage(MainFrame);
    }


    public static BitmapImage? LoadImage(byte[] imageData) {

        if (imageData == null || imageData.Length == 0) return null;

        var image = new BitmapImage();

        using (var mem = new MemoryStream(imageData)) {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }

    // INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnProperty([CallerMemberName] string? name = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}