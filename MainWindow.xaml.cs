// Fichier : MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SAE_201_LOXAM
{
    public partial class MainWindow : Window
    {
        public Agence LAgence { get; set; }
        bool open = true;

        public MainWindow()
        {
            InitializeComponent();
            
        }
        private Reserver currentReserverControl;

        public void ChargeData()
        {
            try
            {
                LAgence = new Agence("Agence 1");
                this.DataContext = LAgence;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème lors de la récupération des données: {ex.Message}\nStack Trace: {ex.StackTrace}");
                Application.Current.Shutdown();
            }
        }

        public void CacheMainWindow()
        {
            var elements = TrouverElementsParTag(this, "MainTag");
            foreach (var element in elements)
            {
                element.Visibility = Visibility.Hidden;
            }
        }

        public void MontreMainWindow()
        {
            var elements = TrouverElementsParTag(this, "MainTag");
            foreach (var element in elements)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        public void AfficherContenu(UserControl uc)
        {
            this.MainContent.Content = uc;
        }

        private List<FrameworkElement> TrouverElementsParTag(DependencyObject parent, object tag)
        {
            var elements = new List<FrameworkElement>();
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var element = VisualTreeHelper.GetChild(parent, i);
                if (element is FrameworkElement fe && Equals(fe.Tag, tag))
                {
                    elements.Add(fe);
                }
                elements.AddRange(TrouverElementsParTag(element, tag));
            }
            return elements;
        }

        private void Reserver_Click(object sender, RoutedEventArgs e)
        {
            CacheMainWindow();
            MainContent.Content = new Reserver();
      
        }

        private void Vérifier_Click(object sender, RoutedEventArgs e)
        {
            CacheMainWindow();
            MainContent.Content = new Verifier();
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            CacheMainWindow();
            MainContent.Content = new Consulter();
        }

        private void Se_connecter_Click(object sender, RoutedEventArgs e)
        {
            var ConnexionFenetre = new Connexion(this);
            ConnexionFenetre.Show();
        }

        private void Acceuil_button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = null;
            MontreMainWindow();
           
        }

        public void SetUserConnected(string username)
        {
            Se_connecter_Button.Content = username;
            Se_connecter_Button.IsEnabled = false;
        }

    }
}