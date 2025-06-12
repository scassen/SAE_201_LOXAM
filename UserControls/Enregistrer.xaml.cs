using Npgsql;
using SAE_201_LOXAM.Classes;
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
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var debut = DateDebut.SelectedDate;
            var fin = DateFin.SelectedDate;
            if ((fin.Value - debut.Value).TotalDays > 5)
            { MessageBox.Show("une reservation ne peut pas durer plus de 5 jours"); }
            if ((fin.Value < debut.Value) )
            { MessageBox.Show("une date de debut ne peut pas etre apres une date de fin"); }
            if (SelectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations))
            {
                  //  Reservation resa = new Reservation(GetNextNumReservation(),DateTime.Now,(DateTime)debut, (DateTime)fin, ((decimal)((fin.Value - debut.Value).TotalDays) * SelectedMateriel.PrixJournee), mainWindow.LAgence.Employes[1],,SelectedMateriel);
                  //  mainWindow.LAgence.Reservations.Add(); 
                }
            else
            { MessageBox.Show("materiel indisponible"); }
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
