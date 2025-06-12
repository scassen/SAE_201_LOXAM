using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Consulter.xaml
    /// </summary>
    public partial class Consulter : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Materiel> materiels;
        private string? referenceFiltre = null;

        public ObservableCollection<Materiel> Materiels
        {
            get => materiels;
            set
            {
                materiels = value;
                OnPropertyChanged(nameof(Materiels));
            }
        }

        public ICollectionView ViewConsulter { get; set; }
        public ICollectionView ViewRetourner { get; set; }

        public Consulter()
        {
            InitializeComponent();
            DataContext = this;

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(mainWindow.LAgence));
            }
            else
            {
                Materiels = new ObservableCollection<Materiel>();
            }

            ViewConsulter = CollectionViewSource.GetDefaultView(Materiels);
            ViewConsulter.Filter = RechercheMotCleMateriel;
            dgConsulter.ItemsSource = ViewConsulter;

            ViewRetourner = new ListCollectionView(Materiels);
            ViewRetourner.Filter = RechercheMotCleMaterielRetourner;
            dgRetourner.ItemsSource = ViewRetourner;
        }

        private bool RechercheMotCleMateriel(object obj)
        {
            if (obj is not Materiel unMateriel)
                return false;

            if (unMateriel.EtatMateriel != Etat.EnMaintenance)
                return false;

            if (!string.IsNullOrEmpty(referenceFiltre) && unMateriel.Reference != referenceFiltre)
                return false;

            if (string.IsNullOrEmpty(Filtre.Text))
                return true;

            return (unMateriel.NomMateriel.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.LibelleType.StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.CategorieType.ToString().StartsWith(Filtre.Text, StringComparison.OrdinalIgnoreCase));
        }


        private bool RechercheMotCleMaterielRetourner(object obj)
        {
            if (obj is not Materiel unMateriel)
                return false;

            if (unMateriel.EtatMateriel != Etat.Disponible)
                return false;

            if (!string.IsNullOrEmpty(referenceFiltre) && unMateriel.Reference != referenceFiltre)
                return false;

            if (string.IsNullOrEmpty(filtre2.Text))
                return true;

            return (unMateriel.NomMateriel.StartsWith(filtre2.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.LibelleType.StartsWith(filtre2.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.TypeMateriel.CategorieType.ToString().StartsWith(filtre2.Text, StringComparison.OrdinalIgnoreCase));
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Sans_rd.IsChecked == true)
                referenceFiltre = "Sans";
            else if (Essence.IsChecked == true)
                referenceFiltre = "Essence";
            else if (rd_Electrique.IsChecked == true)
                referenceFiltre = "Électrique";
            else if (rd_BiEnergie.IsChecked == true)
                referenceFiltre = "Bi énergie";
            else if (rd_tout.IsChecked == true)
                referenceFiltre = null;
            else
                referenceFiltre = null;

            ViewConsulter?.Refresh();
            ViewRetourner?.Refresh();
        }


        private void Filtre_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewConsulter?.Refresh();
        }

        private void filtre2_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewRetourner?.Refresh();
        }

        private void location_Click(object sender, RoutedEventArgs e)
        {
            if (dgConsulter.SelectedItem is not Materiel selectedMateriel)
            {
                MessageBox.Show("Veuillez sélectionner un matériel à remettre en location.");
                return;
            }

            selectedMateriel.EtatMateriel = Etat.EnMaintenance;

            try
            {
                selectedMateriel.UpdateEtatEnMaintenance();
                RafraichirMateriels();
                MessageBox.Show("Le matériel a été remis en location avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour de l'état du matériel : " + ex.Message);
            }
        }

        private void reparer_Click(object sender, RoutedEventArgs e)
        {
            if (dgRetourner.SelectedItem is not Materiel selectedMateriel)
            {
                MessageBox.Show("Veuillez sélectionner un matériel à mettre à réparer.");
                return;
            }

            selectedMateriel.EtatMateriel = Etat.Disponible;

            try
            {
                selectedMateriel.UpdateEtatDispo();
                RafraichirMateriels();
                MessageBox.Show("Le matériel a été remis en réparation avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour de l'état du matériel : " + ex.Message);
            }
        }




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void RafraichirMateriels()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.LAgence is not null)
            {
                var materielsFromDb = new Materiel().FindAll(mainWindow.LAgence);
                Materiels.Clear();
                foreach (var m in materielsFromDb)
                    Materiels.Add(m);
            }

            ViewConsulter.Refresh();
            ViewRetourner.Refresh();
        }

        private void Commentaire_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
