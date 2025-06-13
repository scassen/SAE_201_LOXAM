
using SAE_201_LOXAM.Classes;
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
            dgReserver.Items.Filter = RechercheMateriel;
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

        private bool RechercheMateriel(object obj)
        {
            Materiel unMateriel = obj as Materiel;

            DateTime? dateDebut = FiltreDebut.SelectedDate;
            DateTime? dateFin = FiltreFin.SelectedDate;
            bool filtreTexte = true;
            bool filtreDate = true;

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                List<Reservation> reservations = new Reservation().FindAllAvecIdMateriel(mainWindow.LAgence, unMateriel.NumMateriel);

                if (!String.IsNullOrEmpty(Filtre.Text))
                {
                    filtreTexte =
                        unMateriel.NomMateriel.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase) ||
                        unMateriel.TypeMateriel.LibelleType.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase) ||
                        unMateriel.TypeMateriel.CategorieType.ToString().StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase);
                }

                if (dateDebut.HasValue && dateFin.HasValue)
                {
                    if (reservations.Count == 0)
                    {
                       
                        filtreDate = true;
                    }
                    else
                    {

                        bool overlap = false;

                        foreach (Reservation res in reservations)
                        {
                            if (res.DateDebutLocation < dateFin && res.DateRetourEffectiveLocation > dateDebut)
                            {
                                overlap = true;
                                break;
                            }
                        }

                        filtreDate = !overlap;

                        filtreDate = !overlap;
                    }
                }
            }

            return filtreTexte && filtreDate;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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

        private void Filtre_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgReserver.ItemsSource).Refresh();
        }


    }
}
