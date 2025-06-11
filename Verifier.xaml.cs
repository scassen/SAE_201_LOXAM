using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

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
            get => clients;
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }
        private ObservableCollection<Reservation> reservations;
        public ObservableCollection<Reservation> Reservations
        {
            get => reservations;
            set
            {
                reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        private ICollectionView filteredClients;
        public ICollectionView FilteredClients
        {
            get => filteredClients;
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
                
            }

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
                FilteredClients.Filter = obj =>
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
    }
}
