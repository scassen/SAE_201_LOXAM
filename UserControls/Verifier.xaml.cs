﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{

    public partial class Verifier : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get
            {
                return clients;
            }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        private ObservableCollection<Reservation> reservations;
        public ObservableCollection<Reservation> Reservations
        {
            get
            {
                return reservations;
            }
            set
            {
                reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        private ICollectionView filteredClients;
        public ICollectionView FilteredClients
        {
            get
            {
                return filteredClients;
            }
            set
            {
                filteredClients = value;
                OnPropertyChanged(nameof(FilteredClients));
            }
        }

        public Verifier()
        {
            InitializeComponent();

            this.DataContext = this;

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                Clients = new ObservableCollection<Client>(new Client().FindAll());
                Reservations = new ObservableCollection<Reservation>(new Reservation().FindAll(mainWindow.LAgence));

            }
            else
            {
                Clients = new ObservableCollection<Client>();
                Reservations = new ObservableCollection<Reservation>();
            }
            dgVerifier.ItemsSource = Reservations;
            dgVerifier.Items.Filter = RechercheNumReservation;
            FilteredClients = CollectionViewSource.GetDefaultView(Clients);
        }

        private void ClientLocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ClientLocationTextBox.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredClients.Filter = null;
            }
            else
            {
                FilteredClients.Filter = delegate (object obj)
                {
                    if (obj is Client client)
                    {
                        return client.NomClient.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                };
            }

            FilteredClients.Refresh();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        private void ClientReservationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgVerifier.ItemsSource).Refresh();
        }
        private bool RechercheNumReservation(object obj)
        {
            if (obj is not Reservation reservation)
                return false;

            bool filtreClient = true;
            bool filtreReservation = true;


            if (!string.IsNullOrWhiteSpace(ClientReservationTextBox.Text))
            {
                if (int.TryParse(ClientReservationTextBox.Text, out int num))
                {
                    filtreReservation = reservation.NumReservation == num;
                }
            }


            if (!string.IsNullOrWhiteSpace(ClientLocationTextBox_Copy.Text))
            {
                filtreClient = reservation.Client.NomClient.StartsWith(ClientLocationTextBox_Copy.Text, StringComparison.OrdinalIgnoreCase);
            }

            return filtreReservation && filtreClient;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgVerifier.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une reservation", "Attention",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer cette réservation ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Reservation resaSelectionne = (Reservation)dgVerifier.SelectedItem;

                    try
                    {
                        resaSelectionne.Delete();
                        Reservations.Remove(resaSelectionne);
                        if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
                        {
                            mainWindow.LAgence.Reservations.Remove(resaSelectionne);



                        }
                        MessageBox.Show("La reservation a bien été supprimée.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("La reservation n'a pas pu être supprimée.", "Attention",
                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }





        }
        private void ClientLocationReservationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgVerifier.ItemsSource).Refresh();
        }
        private void Button_Click_Valider(object sender, RoutedEventArgs e)
        {
            if (dgVerifier.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une reservation", "Attention",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Reservation resaSelectionne = (Reservation)dgVerifier.SelectedItem;
                if (resaSelectionne.DateDebutLocation.Date != DateTime.Today)
                {
                    MessageBoxResult result = MessageBox.Show(
                    "Cette réservation n'est pas censée démarrer aujourd'hui, êtes-vous sûr de vouloir la confirmer?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {


                        try
                        {
                            Materiel materiel = resaSelectionne.Materiel;
                            materiel.UpdateEtatLoue();

                            MessageBox.Show("La reservation a bien été validée");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("La reservation n'a pas pu être validée.", "Attention",
                           MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    try
                    {
                        Materiel materiel = resaSelectionne.Materiel;
                        materiel.UpdateEtatLoue();

                        MessageBox.Show("La reservation a bien été validée");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("La reservation n'a pas pu être validée.", "Attention",
                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }
    }
}
