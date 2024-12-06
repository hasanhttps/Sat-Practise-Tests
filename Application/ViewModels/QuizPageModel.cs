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
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using static Application.Models.DatabaseNamespace.Database;

namespace Application.ViewModels;

public class QuizPageModel : INotifyPropertyChanged {

    // Private Fields

    public Module module;
    private Desmos desmos;
    private Frame MainFrame;
    private Exam CurrentExam;
    private ListView AnswersLV;
    private bool calcOpen = false;
    private int questionIndex = 0;
    private DispatcherTimer timer;
    private TimeSpan remainingTime;
    private string remainedTimeTxt;
    private Question currentQuestion;
    private TextBox OpenEndedAnswerTB;
    private BitmapImage questionImage;
    private int selectedQuestionNumber = 1;
    private ObservableCollection<int> questionNumbers;
    private DispatcherTimer topmostTimer = new DispatcherTimer();
    private Dictionary<Question, Answer> KeyValuePairs = new();

    // Binding Properties

    public ICommand CalcButtonCommand { get; set; }
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

    public ObservableCollection<int> QuestionsNumbers { get => questionNumbers; 
        set { 
            questionNumbers = value;
            OnProperty();
        } 
    }

    public int SelectedQuestionNumber { get => selectedQuestionNumber;
        set { 
            selectedQuestionNumber = value;
            questionIndex = selectedQuestionNumber - 1;
            SetQuestions();
            OnProperty();
        }
    }

    // Constructor

    public QuizPageModel(Frame frame, Exam exam, ListView answers, TextBox openEndedAnswerBox) { 
        
        MainFrame = frame;
        CurrentExam = exam;
        AnswersLV = answers;
        OpenEndedAnswerTB = openEndedAnswerBox;

        CalcButtonCommand = new RelayCommand(Calc);
        NextButtonCommand = new RelayCommand(NextQuestion);
        PreviousButtonCommand = new RelayCommand(PreviousQuestion);
        SelectionChangedCommand = new RelayCommand(SelectionChanged);

        QuestionsNumbers = new ObservableCollection<int>();

        StartExam();
    }

    // Functions

    private void Window_Closed(object sender, System.EventArgs e) {
        calcOpen = false;
    }

    private void Calc(object? param) {
        try {
            if (!calcOpen) {
                desmos = new Desmos("https://www.desmos.com/calculator?lang=tr");
                desmos.Topmost = true;
                calcOpen = true;
                desmos.Show();
            }
            else {
                var handle = new System.Windows.Interop.WindowInteropHelper(desmos).Handle;
                NativeMethods.SetWindowPos(handle, (IntPtr)NativeMethods.HWND_TOPMOST, 0, 0, 0, 0,
                    NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE);
                desmos.Topmost = true;
                desmos.Show();
            }
            desmos.Closed += Window_Closed!;
        }
        catch (Exception ex) {
            File.WriteAllText("error.log", ex.ToString());
        }

    }

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
        if (Module.Subject == "Break Time") {
            CurrentQuestion = new Question() { Module = Module, Answers = new List<Answer>() { new Answer(), new Answer() , new Answer() , new Answer() } };
            return;
        }
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

        QuestionsNumbers.Clear();
        var list = Module.Questions.Select(q => q.QuestionNumber).ToList();
        list.ForEach(q => QuestionsNumbers.Add(q));
    }

    public void SetNextModule() {
        // CurrentExam.Modules.Where(p => p.Subject == "Sat Math" && p.ModuleNumber == 1).First();
        questionIndex = 0;
        IfOpenEndedSetAnswer();
        if (Module.Subject == "Sat Verbal" && Module.ModuleNumber == 1) Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Verbal" && p.ModuleNumber == 2).First();
        else if (Module.Subject == "Sat Verbal" && Module.ModuleNumber == 2) Module = new Module() { Exam = CurrentExam, Subject = "Break Time", Time = 10, Questions = new List<Question>() { new Question() {} } };
        else if (Module.Subject == "Break Time") Module = CurrentExam.Modules.Where(p => p.Subject == "Sat Math" && p.ModuleNumber == 1).First();
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