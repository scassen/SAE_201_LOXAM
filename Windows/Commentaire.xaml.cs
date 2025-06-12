using System.Windows;
using Npgsql;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{
    public partial class Commentaire : Window
    {
        // Variable d'instance pour stocker le matériel courant
        private Materiel materiel;

        // Constructeur avec paramètre Materiel
        public Commentaire(Materiel mat)
        {
            InitializeComponent();
            materiel = mat;
            // Pour que le binding fonctionne, on met le DataContext à materiel
            this.DataContext = materiel;
        }

        private void Valider_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (materiel == null)
                {
                    MessageBox.Show("Le matériel n'est pas défini.");
                    return;
                }

                // On récupère le commentaire depuis la propriété liée
                string commentaire = materiel.Commentaire ?? "";

                var cmd = new NpgsqlCommand("UPDATE \"main\".materiel SET commentaire = @commentaire WHERE nummateriel = @nummateriel");
                cmd.Parameters.AddWithValue("@commentaire", commentaire);
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
