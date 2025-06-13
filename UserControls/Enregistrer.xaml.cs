using Npgsql;
using SAE_201_LOXAM.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public decimal PrixTotal { get; set; }
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
                        if (SelectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations)&&SelectedMateriel.EtatMateriel != Etat.EnMaintenance )
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
                MessageBox.Show("Il faut sélectionner un client.");
                return;
            }
            else
            {

                Client selectedClient = client.SelectedItem as Client;
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow == null)
                    return;

                var debut = DateDebut.SelectedDate;
                var fin = DateFin.SelectedDate;

                if (!debut.HasValue || !fin.HasValue)
                {
                    MessageBox.Show("Veuillez sélectionner une date de début et une date de fin.");
                    return;
                }

                if ((fin.Value - debut.Value).TotalDays > 5)
                {
                    MessageBox.Show("Une réservation ne peut pas durer plus de 5 jours.");
                    return;
                }

                if (fin.Value < debut.Value)
                {
                    MessageBox.Show("La date de début ne peut pas être après la date de fin.");
                    return;
                }

                if (!SelectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations))
                {
                    MessageBox.Show("Matériel indisponible.");
                    return;
                }

                Reservation resa = new Reservation(
                    GetNextNumReservation(),
                    DateTime.Now,
                    debut.Value,
                    fin.Value,
                    CalculPrixTotal(debut.Value, fin.Value),
                    mainWindow.LAgence.Employes[0],
                    selectedClient,
                    SelectedMateriel
                );

                try
                {
                    resa.NumReservation = resa.Create();
                    mainWindow.LAgence.Reservations.Add(resa);
                    PrixTotal = CalculPrixTotal((DateTime)DateDebut.SelectedDate, (DateTime)DateFin.SelectedDate);
                    MessageBox.Show("La réservation numero " +resa.NumReservation+ " avec un prix de "+ PrixTotal +  " a bien été confirmée.");

                }
                catch (Exception)
                {
                    MessageBox.Show("La réservation n'a pas pu être créée.");
                }
            }
        }
        private decimal CalculPrixTotal(DateTime debut, DateTime fin)
        {
            return (decimal)(fin - debut).TotalDays * SelectedMateriel.PrixJournee;
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
