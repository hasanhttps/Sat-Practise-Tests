   M S B A M L   `   `   `  ���� e  aMicrosoft.Web.WebView2.Wpf, Version=1.0.2903.40, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf  F BApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Application.ViewsApplication.Views     Application.Views.QuizPage   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  Vwv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf  +localclr-namespace:Application.Views    N� Title$  QuizPage�� N� 
Background
 �����.���  � RowDefinitions �� $
��1.1* ��� $��9* �.+��� $��Center=�.+�� .+���  �� Orientation$ 
Horizontal=�$��Center=�$
��Left=�$��60 0 0 0q�.+��� $��20(�$��Medium"�
���   �$��Center=�$��Center=����� *Module.Subject+�� $��	, Module ��$��20(�$��Medium"�
���   �$��Center=�$��Center=��� $��20(�$��Medium"�
���   �$��Center=�$��Center=����� *Module.ModuleNumber+�� $��23(�
���   �$��Center=�$��Center=����� *RemainingTimeTxt+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� $��Right=�$��Center=�$
��Calc��
�������
���   �$��0 0 60 0q����� *CalcButtonCommand+�� -   $��comboBox��$��40��$��25��$��0 10 0 0q����� *QuestionsNumbers+$ PropertyChanged=�$ TwoWay=�� $��1�� � ColumnDefinitions �� �� .+��� $��	60 40 0 0q�.+��� $	��400��$
��Left=����� *QuestionImage+�� $ 
Horizontal=�.+��� 
���   �$��15(����� * CurrentQuestion.QuestionNumber+�� $��. ��
���   �$��15(��� $
��Wrap=�$	��600��
���   �$��15(����� *CurrentQuestion.QuestionText+�� 	 �� Y1$ 0��	 �� Y2$
 1381��$��Right=�$��Center=�
���   �$	��0.3���� $��1��$��40 40 100 0q�.+��� -   	 ��Name$	 	AnswersLV��$	��600������ *CurrentQuestion.Answers+���� .R��� $	��600��
 �� Padding$
 8q�$
��auto�� �� CornerRadius$ 5��
�������
�������$	��0.5q�._��� $ 
Horizontal=�$��5 0 0 0q�$	��600��.+��� $��Center=�$��15(�
���   ����� *	Variant+�� $��) ��$��Center=�$��15(�
���   ��� $
��Wrap=�$	��550��$��Center=�$��15(�
���   ����� *
AnswerText+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ SelectionChanged��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command �� *SelectionChangedCommand+   CommandParameter �� *SelectedItem+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" ���� -    ��Name$ OpenEndedAnswerTB��$y�	Collapsed=�$	��630��)  #MaterialDesignThemes.Wpf.HintAssist   
Background
 �����$	��100��$��0 0 20 0q�$	��Top=�   Hint$ Answer�� �� AcceptsReturn$
 True�� !  MaterialDesignOutlinedTextBox#��[   �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=��� $ 
Horizontal=�$��Right=�$��	0 200 0 0q�.+��� $��40��$	��150��$��17(�$��0 0 20 0q�$��Right=�$��Previous�� " MaterialDesignRaisedDarkButton#��[ ���� *PreviousButtonCommand+�� $��40��$	��150��$��17(�$��Right=�$
��Next��#��[ ���� *NextButtonCommand+