using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace SAE_201_LOXAM
{
    public partial class Fiche_Client : UserControl
    {
        public Fiche_Client()
        {
            InitializeComponent();
        }

        private void btnCreerClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nom = nom_client_box.Text.Trim();
                string prenom = prenom_client_box.Text.Trim();

                if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom))
                {
                    MessageBox.Show("Le nom et le prénom sont obligatoires.");
                    return;
                }

                int nextNumClient = GetNextNumClient();

                var cmd = new NpgsqlCommand(
                    "INSERT INTO \"MAIN\".client(numclient, nomclient, prenomclient) VALUES (@num, @nom, @prenom)");

                cmd.Parameters.AddWithValue("@num", nextNumClient);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);

                int rowsAffected = DataAccess.Instance.ExecuteSet(cmd);

                if (rowsAffected > 0)
                    MessageBox.Show("Client créé avec succès !");
                else
                    MessageBox.Show("Erreur lors de la création du client.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création du client : " + ex.Message);
            }
        }

        private int GetNextNumClient()
        {
            var cmd = new NpgsqlCommand("SELECT MAX(numclient) FROM \"MAIN\".client");
            object result = DataAccess.Instance.ExecuteSelectUneValeur(cmd);

            if (result != DBNull.Value && result != null)
                return Convert.ToInt32(result) + 1;
            else
                return 10000; // Premier client
        }
    }
}
