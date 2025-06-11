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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_201_LOXAM
{
    /// <summary>
    /// Logique d'interaction pour Verifier.xaml
    /// </summary>
    public partial class Verifier : UserControl
    {
        private ObservableCollection<Reservation> reservations;

        public ObservableCollection<Reservation> Reservations
        {
            get => reservations;
            set
            {
                reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }
        public Verifier()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

     
    }
}
