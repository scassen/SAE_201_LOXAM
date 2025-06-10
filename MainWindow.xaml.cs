using System;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Agence LAgence { get; set; }
        bool open = true;
        public MainWindow()
        {
         
            InitializeComponent();
           
           
        }
        public void ChargeData()
        {
            try
            {
                LAgence = new Agence("Agence 1");
                this.DataContext = LAgence;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données, veuillez consulter votre admin");

                Application.Current.Shutdown();
            }
        }

        public void CacheMainWindow()
        {
            var elements = TrouverElementsParTag(this, "MainTag");
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
            }

            foreach (var element in elements)
            {
                element.Visibility = Visibility.Hidden;
            }
        }
        public void AfficherContenu(UserControl uc)
        {
            this.MainContent.Content = uc;
        }

        public void MontreMainWindow()
        {
            var elements = TrouverElementsParTag(this, "MainTag");
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
            }

            foreach (var element in elements)
            {
                element.Visibility = Visibility.Visible;
            }
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
            var ConnexionFenetre = new Connexion();
            ConnexionFenetre.Show();
        }

        private void Acceuil_button_Click(object sender, RoutedEventArgs e)
        {
            MontreMainWindow();
            MainContent.Content = null;
        }
  


      
    }
}