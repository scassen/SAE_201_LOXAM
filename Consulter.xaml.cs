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
    /// Logique d'interaction pour Consulter.xaml
    /// </summary>
    public partial class Consulter : UserControl
    {
        private ObservableCollection<Materiel> materiels;

        public ObservableCollection<Materiel> Materiels
        {
            get => materiels;
            set
            {
                materiels = value;
                OnPropertyChanged(nameof(Materiels));
            }
        }

        public Consulter()
        {
            InitializeComponent();
            dgConsulter.Items.Filter = RechercheMotCleMateriel;
            this.DataContext = this;

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(mainWindow.LAgence));
            }
            else
            {
                Materiels = new ObservableCollection<Materiel>();
            }
        }

        private bool RechercheMotCleMateriel(object obj)
        {
            if (obj is not Materiel unMateriel)
                return false;

            
            if (unMateriel.EtatMateriel != Etat.EnMaintenance)
                return false;

            if (string.IsNullOrEmpty(Filtre.Text))
                return true;

            return (unMateriel.NomMateriel.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.LibelleType.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.CategorieType.ToString().StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

     

        private void Filtre_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgConsulter.ItemsSource).Refresh();
        }

        private void location_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
