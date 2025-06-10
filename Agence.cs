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
            try
            {
                this.Clients = new ObservableCollection<Client>(new Client().FindAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Clients: {ex.Message}");
            }

            try
            {
                this.Employes = new ObservableCollection<Employe>(new Employe().FindAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Employes: {ex.Message}");
            }

            try
            {
                this.Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(this));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Materiels: {ex.Message}");
            }

            try
            {
                this.Reservations = new ObservableCollection<Reservation>(new Reservation().FindAll(this));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Reservations: {ex.Message}");
            }

            try
            {
                this.Types = new ObservableCollection<Type>(new Type().FindAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Types: {ex.Message}");
            }
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

        public ObservableCollection<Employe> Employes
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



        public ObservableCollection<Materiel> Materiels
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

        public ObservableCollection<Reservation> Reservations
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

        public ObservableCollection<Type> Types
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
