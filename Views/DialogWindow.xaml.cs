using System.Windows;
using Ruletka.Logic;
using Ruletka.Models;

namespace Ruletka.Views
{
    public partial class DialogWindow : Window
    {
        private GameEngine _engine;
        private DialogModel _currentDialog; 

        public DialogWindow(GameEngine engine, DialogModel dialog)
        {
            InitializeComponent();
            _engine = engine;
            _currentDialog = dialog;

           
            QuestionTxt.Text = _currentDialog.Question;
            Option1Btn.Content = _currentDialog.Option1;
            Option2Btn.Content = _currentDialog.Option2;
        }

        private void Option1Btn_Click(object sender, RoutedEventArgs e)
        {
            _engine.Death.Sympathy += _currentDialog.Sympathy1;
            MessageBox.Show($"Wybrałeś opcję 1. (Zmiana sympatii: {_currentDialog.Sympathy1})");
            this.Close();
        }

        private void Option2Btn_Click(object sender, RoutedEventArgs e)
        {
            _engine.Death.Sympathy += _currentDialog.Sympathy2;
            MessageBox.Show($"Wybrałeś opcję 2. (Zmiana sympatii: {_currentDialog.Sympathy2})");
            this.Close();
        }
    }
}