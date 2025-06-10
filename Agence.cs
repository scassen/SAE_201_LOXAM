using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    public class Agence
    {
        private string nom;
        private ObservableCollection<Employe> employes;
        private ObservableCollection<Client> clients;
   
        private ObservableCollection<Materiel> materiels;
        private ObservableCollection<Reservation> reservations;
        private ObservableCollection<Type> types;

        public Agence(string nom)
        {
            this.Nom = nom;
            this.Clients = new ObservableCollection<Client>(new Client().FindAll());
            this.Employes = new ObservableCollection<Employe>(new Employe().FindAll());
            this.Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(this));
            this.Reservations = new ObservableCollection<Reservation>(new Reservation().FindAll(this));
            this.Types = new ObservableCollection<Type>(new Type().FindAll());
        }

        public string Nom
        {
            get
            {
                return this.nom;
            }

            set
            {
                this.nom = value;
            }
        }

        public ObservableCollection<Client> Clients
        {
            get
            {
                return this.clients;
            }

            set
            {
                this.clients = value;
            }
        }

        internal ObservableCollection<Employe> Employes
        {
            get
            {
                return this.employes;
            }

            set
            {
                this.employes = value;
            }
        }



        internal ObservableCollection<Materiel> Materiels
        {
            get
            {
                return this.materiels;
            }

            set
            {
                this.materiels = value;
            }
        }

        internal ObservableCollection<Materiel> Materiels
        {
            get
            {
                return this.reservations;
            }

            set
            {
                this.reservations = value;
            }
        }

        internal ObservableCollection<Type> Types
        {
            get
            {
                return this.types;
            }

            set
            {
                this.types = value;
            }
        }
    }
}
