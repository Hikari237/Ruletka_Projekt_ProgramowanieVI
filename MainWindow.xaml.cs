using System;
using System.Threading.Tasks;
using System.Windows;
using Ruletka.Logic;
using Ruletka.Models;
using Ruletka.Views;
using Ruletka.Database;

namespace Ruletka
{
    public partial class MainWindow : Window
    {
        private GameEngine _engine;
        private Random _globalRandom = new Random();
        private DialogManager _dialogManager = new DialogManager();

        
        public MainWindow(Character player)
        {
            InitializeComponent();
            _engine = new GameEngine(player.Name);

            
            _engine.Player = player;
            _engine.Player.Health = 100; 

            _engine.Death.Sympathy = _engine.Player.Sympathy;
         
            if (_engine.Player.Inventory.Count == 0)
            {
                _engine.Player.Inventory.Add(new Item("Brudny Bandaż (+30HP)", 30, 0));
                _engine.Player.Inventory.Add(new Item("Dobre Cygaro", -5, 15, false));
                _engine.Player.Inventory.Add(new Item("Zardzewiała Piersiówka (+15HP)", 15, 5));
            }

            UpdateUI();
        }
        private void UpdateUI()
        {
            PlayerNameTxt.Text = _engine.Player.Name;
            PlayerHpTxt.Text = $"HP: {_engine.Player.Health}";
            DeathHpTxt.Text = $"HP: {_engine.Death.Health}";


            InventoryComboBox.ItemsSource = null;
            InventoryComboBox.ItemsSource = _engine.Player.Inventory;

            if (_engine.Player.Inventory.Count == 0)
            {
                InventoryComboBox.IsEnabled = false;
                UseItemBtn.IsEnabled = false;
            }
            else
            {
                InventoryComboBox.IsEnabled = true;
                UseItemBtn.IsEnabled = true;
            }
        }

       
        private void EndGame(string message)
        {
            _engine.Player.Sympathy = _engine.Death.Sympathy;
            
            DbManager.SaveOrUpdatePlayer(_engine.Player);

            MessageBoxResult result = MessageBox.Show(
                $"{message}\n\nCzy chcesz spróbować jeszcze raz (TAK), czy wrócić do menu (NIE)?",
                "Koniec Pojedynku",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                _engine.Player.Health = 100;
                _engine.Death.Health = 100;
                _engine.Weapon.Reset();
                UpdateUI();
                StatusTxt.Text = "Nowa szansa... Zakręć bębnem.";
            }
            else
            {
                
                LoginWindow menu = new LoginWindow();
                menu.Show();
                this.Close();
            }
        }

        private void UseItemBtn_Click(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item)InventoryComboBox.SelectedItem;
            if (selectedItem == null) return;

            
            _engine.Player.Health += selectedItem.HealAmount;
            if (_engine.Player.Health > 100) _engine.Player.Health = 100;
            _engine.Death.Sympathy += selectedItem.SympathyChange;

            
            string msg = $"Użyto: {selectedItem.Name}.";
            if (selectedItem.HealAmount > 0) msg += $" +{selectedItem.HealAmount} HP.";
            else if (selectedItem.HealAmount < 0) msg += $" Straciłeś {-selectedItem.HealAmount} HP.";
            StatusTxt.Text = msg;

            
            if (selectedItem.IsConsumable)
            {
                _engine.Player.Inventory.Remove(selectedItem);
            }

            UpdateUI();

            
            if (_engine.Player.Health <= 0)
            {
                _engine.Player.Deaths++;
                EndGame("Ostatni mach cygara okazał się śmiertelny...");
                return;
            }

            _engine.NextTurn();
            ProcessDeathTurn();
        }

        private void SpinBtn_Click(object sender, RoutedEventArgs e)
        {
            _engine.SpinCylinder();
            StatusTxt.Text = "Bęben się kręci... Wybierz cel!";
        }

        private void ShootEnemyBtn_Click(object sender, RoutedEventArgs e)
        {
            bool hit = _engine.PullTrigger(_engine.Death);
            StatusTxt.Text = hit ? "TRAFIŁEŚ ŚMIERĆ!" : "Klik... pusto.";
            UpdateUI();

            if (_engine.Death.Health <= 0)
            {
                _engine.Player.Wins++; 
                EndGame("Głupcze... Śmierci nie da się pokonać.");
            }
            else
            {
                _engine.NextTurn();
                ProcessDeathTurn();
            }
        }

        private void ShootSelfBtn_Click(object sender, RoutedEventArgs e)
        {
            bool hit = _engine.PullTrigger(_engine.Player);
            StatusTxt.Text = hit ? "AŁA! Postrzeliłeś się!" : "Uff... pusto.";
            UpdateUI();

            if (_engine.Player.Health <= 0)
            {
                _engine.Player.Deaths++;
                EndGame("Tym razem rewolwer był szybszy od Ciebie...");
            }
            else
            {
                _engine.NextTurn();
                ProcessDeathTurn();
            }
        }

        private async void ProcessDeathTurn()
        {
            SpinBtn.IsEnabled = false;
            ShootEnemyBtn.IsEnabled = false;
            ShootSelfBtn.IsEnabled = false;
            UseItemBtn.IsEnabled = false;

            StatusTxt.Text = "Śmierć wykonuje ruch...";
            await Task.Delay(1500);

            bool shootPlayer = _globalRandom.Next(0, 100) < 70;

            if (shootPlayer)
            {
                StatusTxt.Text = "Śmierć mierzy w Ciebie...";
                await Task.Delay(1000);
                bool hit = _engine.PullTrigger(_engine.Player);
                StatusTxt.Text = hit ? "ŚMIERĆ CIĘ TRAFIŁA!" : "Śmierć chybiła.";
            }
            else
            {
                StatusTxt.Text = "Śmierć mierzy w siebie...";
                await Task.Delay(1000);
                bool hit = _engine.PullTrigger(_engine.Death);
                StatusTxt.Text = hit ? "Śmierć się postrzeliła!" : "Śmierć ma szczęście... pusto.";
            }

            UpdateUI();

            if (_engine.Player.Health <= 0)
            {
                _engine.Player.Deaths++;
                EndGame("Koniec Twojej drogi, kowboju.");
            }
            else
            {
                
                if (_globalRandom.Next(0, 100) < 30)
                {
                    DialogModel randomDialog = _dialogManager.GetRandomDialog();
                    DialogWindow dialog = new DialogWindow(_engine, randomDialog);
                    dialog.Owner = this;
                    dialog.ShowDialog();
                }

                _engine.NextTurn();
                StatusTxt.Text = $"Twoja tura. Sympatia Śmierci: {_engine.Death.Sympathy}";

                SpinBtn.IsEnabled = true;
                ShootEnemyBtn.IsEnabled = true;
                ShootSelfBtn.IsEnabled = true;
                if (_engine.Player.Inventory.Count > 0) UseItemBtn.IsEnabled = true;
            }
        }
    }
}