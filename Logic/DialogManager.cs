using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ruletka.Models;

namespace Ruletka.Logic
{
    public class DialogManager
    {
        private List<DialogModel> _dialogs;
        private Random _random = new Random();

        public DialogManager()
        {
            LoadDialogsFromFile();
        }

        private void LoadDialogsFromFile()
        {

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dialogs.json");

           

                if (File.Exists(filePath))
                {

                    string jsonText = File.ReadAllText(filePath);


                    _dialogs = JsonSerializer.Deserialize<List<DialogModel>>(jsonText);
                }

            
        
            
        }

        

        public DialogModel GetRandomDialog()
        {
            int index = _random.Next(_dialogs.Count);
            return _dialogs[index];
        }
    }
}