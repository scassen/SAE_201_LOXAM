// Fichier : Reserver.xaml.cs
using SAE_201_LOXAM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SAE_201_LOXAM
{
    public partial class Reserver : UserControl, INotifyPropertyChanged
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

        public Reserver()
        {
            InitializeComponent();
            dgReserver.Items.Filter = RechercheMotCleMateriel;
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
            if (String.IsNullOrEmpty(Filtre.Text))
                return true;
            Materiel unMateriel = obj as Materiel;
            return (unMateriel.NomMateriel.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase)
            || unMateriel.TypeMateriel.LibelleType.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase) 
            || unMateriel.TypeMateriel.CategorieType.ToString().StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            var ficheClient = new Fiche_Client();
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.AfficherContenu(ficheClient);
            }
        }

        private void Filtre_TextChanged(object sender, TextChangedEventArgs e)  
        {
            CollectionViewSource.GetDefaultView(dgReserver.ItemsSource).Refresh();
        }
        private void dgReserver_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgReserver.SelectedItem is Materiel selectedMateriel)
            {
                var details = new DetailMateriel(selectedMateriel);
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.AfficherContenu(details);
                }
            }
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            Materiel selectedMateriel = dgReserver.SelectedItem as Materiel;
            if (selectedMateriel != null)
            {
                var enregistrer = new Enregistrer(selectedMateriel);
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.AfficherContenu(enregistrer);
                }
            }
            else
                MessageBox.Show("pas de materiel séléctionné");

        }

    }
}
