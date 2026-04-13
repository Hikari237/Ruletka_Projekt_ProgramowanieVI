using System.Windows;
using Ruletka.Database;
using Ruletka.Models;
using System.Collections.Generic;

namespace Ruletka.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoadPlayersList();
        }

        private void LoadPlayersList()
        {
            List<Character> players = DbManager.GetAllPlayers();
            ExistingPlayersCombo.ItemsSource = players;
        }

        private void ExistingPlayersCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ExistingPlayersCombo.SelectedItem is Character selected)
            {
                PlayerNameInput.Text = selected.Name; // Automatycznie wpisz imię
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            string name = PlayerNameInput.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            // Szukamy czy taki gracz już istnieje w bazie
            List<Character> all = DbManager.GetAllPlayers();
            Character playerToLoad = all.Find(p => p.Name == name);

            if (playerToLoad == null)
            {
                playerToLoad = new Character(name, 100);
                DbManager.SaveOrUpdatePlayer(playerToLoad);
            }

            MainWindow game = new MainWindow(playerToLoad); // PRZEKAZUJEMY CAŁEGO GRACZA, NIE TYLKO IMIĘ
            game.Show();
            this.Close();
        }

        private void ShowRankingBtn_Click(object sender, RoutedEventArgs e)
        {
            // Tworzymy okno rankingu
            RankingWindow ranking = new RankingWindow();

            // Ustawiamy LoginWindow jako właściciela (żeby okienko wyskoczyło na środku)
            ranking.Owner = this;

            // Wyświetlamy jako okno modalne (zatrzymuje tło, dopóki nie zamkniesz rankingu)
            ranking.ShowDialog();
        }
        private void ExitAppBtn_Click(object sender, RoutedEventArgs e)
        {
            // Całkowite zamknięcie całej aplikacji
            Application.Current.Shutdown();
        }
    }
}