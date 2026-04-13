using System.Windows;
using Ruletka.Database;

namespace Ruletka
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
          
            DbManager.InitializeDatabase();
        }
    }
}