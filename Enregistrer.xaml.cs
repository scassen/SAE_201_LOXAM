using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Logique d'interaction pour Enregistrer.xaml
    /// </summary>
     public enum Disponibilite {Disponible,Indisponible}
    public partial class Enregistrer : UserControl,INotifyPropertyChanged
    {

        public ObservableCollection<Materiel> SingleMaterielList { get; set; }
        public Materiel SelectedMateriel { get; set; }
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

        public Disponibilite EstDisponible
        {
            get
            {
                return this.estDisponible;
            }

            set
            {
                if (estDisponible != value)
                {
                    estDisponible = value;
                    OnPropertyChanged(nameof(EstDisponible));
                }
            
            }
        }

        private Disponibilite estDisponible;
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




        public Enregistrer()
        {
            InitializeComponent();
            DataContext = this;
        }
        public Enregistrer(Materiel materiel)
        {

            InitializeComponent();
           
            SelectedMateriel = materiel;
             SingleMaterielList = new ObservableCollection<Materiel>() { materiel};
            DataContext = this;
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                Clients = new ObservableCollection<Client>(new Client().FindAll());
            }
                FilteredClients = CollectionViewSource.GetDefaultView(Clients);



        }
        private void ClientLocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Filtre.Text;

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
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CheckDisponibilite()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var debut = DateDebut.SelectedDate;
                var fin = DateFin.SelectedDate;
                if (debut.HasValue && fin.HasValue)
                {
                    if ((fin.Value - debut.Value).TotalDays > 5)
                    { MessageBox.Show("une reservation ne peut pas durer plus de 5 jours"); }
                    else
                    {
                        if (SelectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations))
                        {
                            EstDisponible = Disponibilite.Disponible;
                          
                        }
                        else
                        {
                            EstDisponible = Disponibilite.Indisponible;
                            
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            if (client.SelectedItem == null)
            {
                MessageBox.Show("il faut selectionner un client");
            }
            else
            {
                Client selectedClient = client.SelectedItem as Client;
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    var debut = DateDebut.SelectedDate;
                    var fin = DateFin.SelectedDate;
                    if ((fin.Value - debut.Value).TotalDays > 5)
                    { MessageBox.Show("une reservation ne peut pas durer plus de 5 jours"); }
                    if ((fin.Value < debut.Value))
                    { MessageBox.Show("une date de debut ne peut pas etre apres une date de fin"); }
                    if (SelectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations))
                    {
                        Reservation resa = new Reservation(GetNextNumReservation(), DateTime.Now, (DateTime)debut, (DateTime)fin, ((decimal)((fin.Value - debut.Value).TotalDays) * SelectedMateriel.PrixJournee), mainWindow.LAgence.Employes[0], selectedClient, SelectedMateriel);
                        try
                        {
                            resa.NumReservation = resa.Create();
                            mainWindow.LAgence.Reservations.Add(resa);
                            MessageBox.Show("la reservation a bien été confirmée");
                        }
                        catch(Exception ex) { MessageBox.Show( "La reservation n'a pas pu être créée."); }
                    }
                    else
                    { MessageBox.Show("materiel indisponible"); }
                }
            }
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDisponibilite();
        }
        private int GetNextNumReservation()
        {
            var cmd = new NpgsqlCommand("SELECT MAX(numreservation) FROM \"main\".reservation");
            object result = DataAccess.Instance.ExecuteSelectUneValeur(cmd);
            return (result != DBNull.Value && result != null) ? Convert.ToInt32(result) + 1 : 10000;
        }


    }
}   
