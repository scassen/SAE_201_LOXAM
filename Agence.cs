// Fichier : Agence.cs
using System;
using System.Collections.ObjectModel;

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
            try { this.Clients = new ObservableCollection<Client>(new Client().FindAll()); } catch (Exception ex) { Console.WriteLine($"Error loading Clients: {ex.Message}"); }
            try { this.Employes = new ObservableCollection<Employe>(new Employe().FindAll()); } catch (Exception ex) { Console.WriteLine($"Error loading Employes: {ex.Message}"); }
            try { this.Materiels = new ObservableCollection<Materiel>(new Materiel().FindAll(this)); } catch (Exception ex) { Console.WriteLine($"Error loading Materiels: {ex.Message}"); }
            try { this.Reservations = new ObservableCollection<Reservation>(new Reservation().FindAll(this)); } catch (Exception ex) { Console.WriteLine($"Error loading Reservations: {ex.Message}"); }
            try { this.Types = new ObservableCollection<Type>(new Type().FindAll()); } catch (Exception ex) { Console.WriteLine($"Error loading Types: {ex.Message}"); }
        }

        public string Nom { get => nom; set => nom = value; }
        public ObservableCollection<Client> Clients { get => clients; set => clients = value; }
        public ObservableCollection<Employe> Employes { get => employes; set => employes = value; }
        public ObservableCollection<Materiel> Materiels { get => materiels; set => materiels = value; }
        public ObservableCollection<Reservation> Reservations { get => reservations; set => reservations = value; }
        public ObservableCollection<Type> Types { get => types; set => types = value; }
    }
}
