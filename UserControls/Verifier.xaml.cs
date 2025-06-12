using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Verifier.xaml
    /// </summary>
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
            if (String.IsNullOrWhiteSpace(ClientReservationTextBox.Text))
                return true;
            if (int.TryParse(ClientReservationTextBox.Text, out int num))
                return (reservation.NumReservation == num);
           return false;
        }
    }
}
