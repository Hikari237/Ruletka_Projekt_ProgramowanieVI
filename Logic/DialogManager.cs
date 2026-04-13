using Microsoft.Data.Sqlite;
using Ruletka.Models;
using System;
using System.Collections.Generic;

namespace Ruletka.Logic
{
    public class DialogManager
    {
        private List<DialogModel> _dialogs;
        private Random _rand = new Random(); // Ten Random stworzony raz, działa poprawnie!

        public DialogManager()
        {
            // Tu wpisujemy wszystkie nasze dialogi
            _dialogs = new List<DialogModel>
            {
                new DialogModel {
                    Question = "Boisz się tego, co po drugiej stronie?",
                    Option1 = "Tak.", Sympathy1 = 10,
                    Option2 = "Nie ma drugiej strony.", Sympathy2 = -10
                },
                new DialogModel {
                    Question = "Dlaczego w ogóle tu przyszedłeś?",
                    Option1 = "Szukam odkupienia.", Sympathy1 = 15,
                    Option2 = "Dla pieniędzy.", Sympathy2 = -5
                },
                new DialogModel {
                    Question = "Zabiłeś już kogoś wcześniej?",
                    Option1 = "Tylko w samoobronie.", Sympathy1 = 5,
                    Option2 = "Przestałem liczyć.", Sympathy2 = -15
                }
            };
        }

        public DialogModel GetRandomDialog()
        {
            
            int index = _rand.Next(_dialogs.Count);
            return _dialogs[index];
        }
        // 3. Pobieranie Top 10 graczy do Rankingu
        public static List<Character> GetTopPlayers()
        {
            var players = new List<Character>();
           
            
            return players;
        }
    }
}