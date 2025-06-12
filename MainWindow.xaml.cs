// Fichier : MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SAE_201_LOXAM
{

    public partial class MainWindow : Window
    {
        public Agence LAgence { get; set; }
        private string userRole;

        public MainWindow()
        {
            InitializeComponent();
            
        }
        private Reserver currentReserverControl;

        public void ChargeData()
        {
            try
            {
                var agence = new Agence("Agence 1");
                Console.WriteLine($"[DEBUG] Employes loaded? {(agence.Employes == null ? "null" : agence.Employes.Count.ToString())}");
                Console.WriteLine($"[DEBUG] Clients loaded? {(agence.Clients == null ? "null" : agence.Clients.Count.ToString())}");
                Console.WriteLine($"[DEBUG] Materiels loaded? {(agence.Materiels == null ? "null" : agence.Materiels.Count.ToString())}");
                if (agence.Employes == null || agence.Clients == null || agence.Materiels == null)
                    throw new InvalidOperationException("Agence is not fully initialized before loading reservations.");
                agence.Reservations = new ObservableCollection<Reservation>(new Reservation().FindAll(agence)
                );
                this.LAgence = agence;
                this.DataContext = LAgence;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème lors de la récupération des données: {ex.Message}\nStack Trace: {ex.StackTrace}");
                Application.Current.Shutdown();
            }
        }
        public void SetUserConnected(string username)
        {
            Se_connecter_Button.Content = username;
            Se_connecter_Button.IsEnabled = false;

            if (username == "noslandm")
            {
                userRole = "responsable";
            }
            else
            {
                userRole = "employe";
            }

            AdapterInterfaceSelonRole();
        }
        private void AdapterInterfaceSelonRole()
        {

            if (userRole == "responsable")
            {
                
                Verifier_button.IsEnabled = true;
                Reserver_button.IsEnabled = true;
                Consulter_button.IsEnabled = true;
            }
            else if (userRole == "employe")
            {
                
                Verifier_button.IsEnabled = true;
                Reserver_button.IsEnabled = true;
                Consulter_button.IsEnabled = true;
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
        /*public void Commentaire(Window win)
        {
            var Com = Commentaire(win)
        }
        */

        

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

    }
}