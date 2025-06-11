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
        public Materiel selectedMateriel { get; set; }
        public Disponibilite EstDisponible { get; set; }
       

        public Enregistrer()
        {
            InitializeComponent();
            DataContext = this;
        }
        public Enregistrer(Materiel materiel)
        {

            InitializeComponent();
           
            selectedMateriel = materiel;
             SingleMaterielList = new ObservableCollection<Materiel>() { materiel};
            DataContext = this;




        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
                        if (selectedMateriel.EstDisponible(debut.Value, fin.Value, mainWindow.LAgence.Reservations))
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

        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDisponibilite();
        }


    }
}   
