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
using System.Windows.Shapes;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : Window
    {
        private MainWindow mainWindow;

        public Connexion(MainWindow mainWin)
        {
            InitializeComponent();
            mainWindow = mainWin;
        }

        private void Button_Valider_Click(object sender, RoutedEventArgs e)
        {
            string username = identifier_box.Text;
            string password = mdp_box.Password;

            string connString = $"Host=srv-peda-new;Port=5433;Username={username};Password={password};Database=201;Options='-c search_path=main'";

            try
            {
                DataAccess.Init(connString);
                var conn = DataAccess.Instance.GetConnection();
                MessageBox.Show("Connexion réussie ! attendez s'il vous plait");

                // Met à jour le bouton dans MainWindow
                mainWindow.SetUserConnected(username);
                mainWindow.ChargeData();
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }
        

    }
}
