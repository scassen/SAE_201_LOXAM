// Fichier : Agence.cs
using System;
using System.Collections.ObjectModel;

namespace SAE_201_LOXAM.Classes
{
    public class Agence
    {
        private string nom;
        private ObservableCollection<Type> types;
        private ObservableCollection<Employe> employes;
        private ObservableCollection<Client> clients;
        private ObservableCollection<Materiel> materiels;
        private ObservableCollection<Reservation> reservations;


        public Agence(string nom)
        {
            this.Nom = nom;
            try { this.Types = new ObservableCollection<Type>(new Type().FindAll()); } catch (Exception ex) { Console.WriteLine($"Error loading Types: {ex.Message}"); }
            try { this.Clients = new ObservableCollection<Client>(new Client().FindAll()); } catch (Exception ex) { Console.WriteLine($"Error loading Clients: {ex.Message}"); this.Clients = new ObservableCollection<Client>(); }
            try
            {
                this.Employes = new ObservableCollection<Employe>(new Employe().FindAll());
                Console.WriteLine($"Loaded {this.Employes.Count} employé(s).");
                foreach (var emp in this.Employes)
                    Console.WriteLine($"  Employé: {emp.NumEmploye} - {emp.NomEmploye} {emp.PrenomEmploye}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Employes: {ex.Message}");
            }
            try { this.Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(this)); } catch (Exception ex) { Console.WriteLine($"Error loading Materiels: {ex.Message}"); this.Materiels = new ObservableCollection<Materiel>(); }


        }

        public string Nom
        {
            get
            {
                return nom;
            }
            set
            {
                nom = value;
            }
        }

        public ObservableCollection<Client> Clients
        {
            get
            {
                return clients;
            }
            set
            {
                clients = value;
            }
        }

        public ObservableCollection<Employe> Employes
        {
            get
            {
                return employes;
            }
            set
            {
                employes = value;
            }
        }

        public ObservableCollection<Materiel> Materiels
        {
            get
            {
                return materiels;
            }
            set
            {
                materiels = value;
            }
        }

        public ObservableCollection<Reservation> Reservations
        {
            get
            {
                return reservations;
            }
            set
            {
                reservations = value;
            }
        }

        public ObservableCollection<Type> Types
        {
            get
            {
                return types;
            }
            set
            {
                types = value;
            }
        }

    }
}
