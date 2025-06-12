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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SAE_201_LOXAM.Classes;

namespace SAE_201_LOXAM
{
    public partial class DetailMateriel : UserControl
    {
        public DetailMateriel(Materiel materiel)
        {
            InitializeComponent();
            DataContext = materiel;
        }
    }
}
