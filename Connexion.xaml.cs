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

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : Window
    {
        public Connexion()
        {
            InitializeComponent();

        }

        private void Button_Valider_Click(object sender, RoutedEventArgs e)
        {
            string username = identifier_box.Text;
            string password = mdp_box.Text;

            string connString = $"Host=srv-peda-new;Port=5433;Username={username};Password={password};Database=201;Options='-c search_path=MAIN'";

            try
            {
                // Initialise la connexion (singleton)
                DataAccess.Init(connString);

                // Test sans using
                var conn = DataAccess.Instance.GetConnection();
                MessageBox.Show("Connexion réussie !");
               
               
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
            this.Close();
              
        }
        

    }
}
