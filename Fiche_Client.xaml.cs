// Fichier : Fiche_Client.xaml.cs
using System;
using System.Collections.Generic;
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

                List<Certification> certifications = new();
                if (interne_check.IsChecked == true) certifications.Add(Certification.Interne);
                if (CACES_R486_check.IsChecked == true) certifications.Add(Certification.CACES_R486);
                if (CACES_R482_check.IsChecked == true) certifications.Add(Certification.CACES_R482);

                int nextNumClient = GetNextNumClient();

                var insertClientCmd = new NpgsqlCommand("INSERT INTO \"MAIN\".client(numclient, nomclient, prenomclient) VALUES (@num, @nom, @prenom)");
                insertClientCmd.Parameters.AddWithValue("@num", nextNumClient);
                insertClientCmd.Parameters.AddWithValue("@nom", nom);
                insertClientCmd.Parameters.AddWithValue("@prenom", prenom);

                int rowsAffected = DataAccess.Instance.ExecuteSet(insertClientCmd);

                foreach (var certif in certifications)
                {
                    var insertCertifCmd = new NpgsqlCommand("INSERT INTO \"MAIN\".dispose(numclient, numcertification) VALUES (@numclient, @numcertif)");
                    insertCertifCmd.Parameters.AddWithValue("@numclient", nextNumClient);
                    insertCertifCmd.Parameters.AddWithValue("@numcertif", (int)certif);
                    DataAccess.Instance.ExecuteSet(insertCertifCmd);
                }

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
            return (result != DBNull.Value && result != null) ? Convert.ToInt32(result) + 1 : 10000;
        }
    }
}
