using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Commentaire.xaml
    /// </summary>
    public partial class Commentaire : Window
    {

        private Materiel materiel;

        public Commentaire(Materiel materiel)
        {
            InitializeComponent();
            DataContext = materiel;

        }

        private void Valider_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Prépare la commande SQL pour mettre à jour le commentaire
                var cmd = new NpgsqlCommand("UPDATE \"main\".materiel SET commentaire = @commentaire WHERE nummateriel = @nummateriel");
                cmd.Parameters.AddWithValue("@commentaire", materiel.Commentaire ?? "");
                cmd.Parameters.AddWithValue("@nummateriel", materiel.NumMateriel);

                int rowsAffected = DataAccess.Instance.ExecuteSet(cmd);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Commentaire mis à jour avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du commentaire.");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour du commentaire : " + ex.Message);
            }
        }
    }
    
}
