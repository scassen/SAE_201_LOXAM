using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Reserver.xaml
    /// </summary>
    public partial class Reserver : UserControl
    {
        public Reserver()
        {
            InitializeComponent();
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {

            var ficheClient = new Fiche_Client();

            // Récupère la fenêtre principale
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.AfficherContenu(ficheClient);
            }
        }

    }
}
