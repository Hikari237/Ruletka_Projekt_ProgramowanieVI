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

            LoadRanking();
        }

        private void LoadRanking()
        {
            
            List<Character> topPlayers = DbManager.GetTopPlayers();

            
            RankingList.ItemsSource = topPlayers;
        }
    }
}