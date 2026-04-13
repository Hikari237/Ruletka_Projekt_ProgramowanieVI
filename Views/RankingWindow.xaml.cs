using System.Collections.Generic;
using System.Windows;
using Ruletka.Database;
using Ruletka.Models;

namespace Ruletka.Views
{
    public partial class RankingWindow : Window
    {
        public RankingWindow()
        {
            InitializeComponent();

            // Przy otwieraniu okna, od razu pobieramy listę z bazy i ładujemy do tabeli
            LoadRanking();
        }

        private void LoadRanking()
        {
            // Pobieramy posortowaną listę 10 graczy
            List<Character> topPlayers = DbManager.GetTopPlayers();

            // Podpinamy listę do naszej tabeli z interfejsu (ListView)
            RankingList.ItemsSource = topPlayers;
        }
    }
}