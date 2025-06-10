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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Reserver_Click(object sender, RoutedEventArgs e)
        {
            var ReserverFenetre = new Reserver();
            ReserverFenetre.Show();
        }

        private void Vérifier_Click(object sender, RoutedEventArgs e)
        {
            var VerifierFenetre = new Verifier();
            VerifierFenetre.Show();
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            var ConsulterFenetre = new Consulter();
            ConsulterFenetre.Show();
        }

        private void Se_connecter_Click(object sender, RoutedEventArgs e)
        {
            var ConnexionFenetre = new Connexion();
            ConnexionFenetre.Show();
        }
    }
}