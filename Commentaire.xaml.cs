using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Commentaire.xaml
    /// </summary>
    public partial class Commentaire : Window
    {
       

        public Commentaire(Materiel materiel)
        {
            InitializeComponent();
            DataContext = materiel;

        }

        private void Valider_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
