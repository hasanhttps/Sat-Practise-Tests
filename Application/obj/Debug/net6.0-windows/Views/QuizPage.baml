   M S B A M L   `   `   `  ����e  aMicrosoft.Web.WebView2.Wpf, Version=1.0.2903.40, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf  5      F BApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Application.ViewsApplication.Views 5	          Application.Views.QuizPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      Vwv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf  5      +localclr-namespace:Application.Views  5	        N� Title$  QuizPage��5       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $
��1.1* �6   �� 5      $��9* �6   .+�5   
   �� $��Center=�6   .+�5      � .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   $
��Left=�6Q   $��60 0 0 0q�6l   .+�5      �� $��20(�6@   $��Medium"�6N   
���   �$��Center=�6u   $��Center=�6�   ��6    �� *Module.Subject+6�   �� 5      $��	, Module ��6    $��20(�61   $��Medium"�6?   
���   �$��Center=�6f   $��Center=�6�   �� 5      $��20(�6E   $��Medium"�6S   
���   �$��Center=�6z   $��Center=�6�   ��6    �� *Module.ModuleNumber+6�   5      �� 5      $��23(�6p   
���   �$��Center=�6�   $��Center=�6�   ��6   �� *RemainingTimeTxt+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�6�   �� 5      $��Right=�6   $��Center=�65   $
��Calc��6P   
�������
���   �$��0 0 60 0q�6�   ��6�   �� *CalcButtonCommand+5      �� 5      -   5        $��comboBox��5      $��40��6'   $��25��62   $��0 10 0 0q�6>   ��6P   �� *QuestionsNumbers+$ PropertyChanged=�$ TwoWay=� �� SelectedItem 6�   �� *SelectedQuestionNumber+5!      � 5#   
   $��1��6    � ColumnDefinitions 5$      �� 5%      �� 5&      .+�5*      �� $��	60 40 0 0q�6   .+�5+      �� $	��400��69   $
��Left=�6I   ��6   �� *QuestionImage+6I   �� 5,      $ 
Horizontal=�6   .+�5-      �� 
���   �$��15(�6c   ��6    �� * CurrentQuestion.QuestionNumber+6c   �� 5.      $��. ��6    
���   �$��15(�6=   �� 5/      $
��Wrap=�6N   $	��600��6b   
���   �$��15(�6�   ��6    �� *CurrentQuestion.QuestionText+6�   50      51      �� 53      	 �� Y1$ 0��6   		 �� Y2$
	 1381��6   $��Right=�6$   $��Center=�6@   
���   �$	��0.3��6j   �� 55      $��1��6   $��40 40 100 0q�6)   .+�56      �� -   5        
 ��Name$
 	AnswersLV��56      $	��600��6^   ��6.   �� *CurrentQuestion.Answers+��57      �� 58      .R�59      �� $	��600��6%    �� Padding$ 8q�61   $
��auto��6=    �� CornerRadius$ 5��6K   
�������
�������$	��0.5q�6�   ._�5:   "   �� $ 
Horizontal=�6-   $��5 0 0 0q�6F   $	��600��6W   .+�5;   &   �� $��Center=�6I   $��15(�6d   
���   ���60   �� *	Variant+6r   �� 5<   &   $��) ��60   $��Center=�6:   $��15(�6U   
���   �6c   �� 5=   &   $
��Wrap=�6L   $	��550��6`   $��Center=�6l   $��15(�6�   
���   ���60   �� *
AnswerText+6�   5>   #   5?      5@      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5C      +  %Microsoft.Xaml.Behaviors.EventTrigger  5D         	EventName$ SelectionChanged��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5E      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 64   �� *SelectionChangedCommand+   CommandParameter 5F   $   �� *SelectedItem+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" ��5G      5I      �� 5K      -   5         ��Name$ OpenEndedAnswerTB��5K      $y�	Collapsed=�65   $	��630��6L   )  #MaterialDesignThemes.Wpf.HintAssist   
Background
 �����$	��100��6�   $��0 0 20 0q�6�   $	��Top=�6�      Hint$ Answer��6�    �� AcceptsReturn$
 True��6�    !  MaterialDesignOutlinedTextBox#��[  6�    �� TextWrapping$
 Wrap=�60  " �� VerticalScrollBarVisibility$
 Auto=�6D  �� 5M      $ 
Horizontal=�6   $��Right=�66   $��	0 200 0 0q�6R   .+�5O      �� $��40��5P      $	��150��5Q      $��17(�5R      $��0 0 20 0q�5S      $��Right=�5U      $��Previous��5V       " MaterialDesignRaisedDarkButton#��[ 5W      ��5T      �� *PreviousButtonCommand+5W      �� 5Y      $��40��5Z      $	��150��5[      $��17(�5\      $��Right=�5^      $
��Next��5_      #��[ 5`      ��5]      �� *NextButtonCommand+5`      5b      5c      5d      5e      5f      