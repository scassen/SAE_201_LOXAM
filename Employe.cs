// Fichier : Employe.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAE_201_LOXAM
{
    public enum Role { Employe, ResponsableAtelier }

    public class Employe : ICrud<Employe>, INotifyPropertyChanged
    {
        private int numEmploye;
        private string nomEmploye;
        private string prenomEmploye;
        private string login;
        private string mdp;
        private Role roleEmploye;

        public Employe() { }

        public int NumEmploye { get => numEmploye; set => numEmploye = value; }
        public string NomEmploye
        {
            get => nomEmploye;
            set => nomEmploye = value.Length > 30 ? throw new ArgumentOutOfRangeException("Nom de moins de 30 caractères") : value;
        }
        public string PrenomEmploye
        {
            get => prenomEmploye;
            set => prenomEmploye = value.Length > 30 ? throw new ArgumentOutOfRangeException("Prénom de moins de 30 caractères") : value;
        }
        public string Login
        {
            get => login;
            set => login = value.Length > 30 ? throw new ArgumentOutOfRangeException("Login de moins de 30 caractères") : value;
        }
        public string Mdp
        {
            get => mdp;
            set => mdp = value.Length > 30 ? throw new ArgumentOutOfRangeException("MDP de moins de 30 caractères") : value;
        }
        public Role RoleEmploye { get => roleEmploye; set => roleEmploye = value; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create() => throw new NotImplementedException();
        public int Delete() => throw new NotImplementedException();
        public void Read() => throw new NotImplementedException();
        public int Update() => throw new NotImplementedException();
        public List<Employe> FindAll() => throw new NotImplementedException();
        public List<Employe> FindBySelection(string criteres) => throw new NotImplementedException();
    }
}
