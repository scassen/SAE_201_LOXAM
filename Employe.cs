using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    public enum Role {Employe,ResponsableAtelier }
    internal class Employe:ICrud<Employe>,INotifyPropertyChanged
    {
        private int numEmploye;
        private string nomEmploye;
        private string prenomEmploye;
        private string login;
        private string mdp;
        private Role roleEmploye;

        public Employe()
        {
        }

        public int NumEmploye
        {
            get
            {
                return this.numEmploye;
            }

            set
            {

                this.numEmploye = value;
            }
        }

        public string NomEmploye
        {
            get
            {
                return this.nomEmploye;
            }

            set
            {
                if (value.Length > 30)
                { throw new ArgumentOutOfRangeException("nom de moins de 30 caracteres"); }
                else
                    this.nomEmploye = value;
            }
        }

        public string PrenomEmploye
        {
            get
            {

                    return this.prenomEmploye;
            }

            set
            {
                if (value.Length > 30)
                { throw new ArgumentOutOfRangeException("prenom de moins de 30 caracteres"); }
                else
                    this.prenomEmploye = value;
            }
        }

        public string Login
        {
            get
            {
                return this.login;
            }

            set
            {
                if (value.Length > 30)
                { throw new ArgumentOutOfRangeException("login de moins de 30 caracteres"); }
                else
                    this.login = value;
            }
        }

        public string Mdp
        {
            get
            {
                return this.mdp;
            }

            set
            {
                if (value.Length > 30)
                { throw new ArgumentOutOfRangeException("mdp de moins de 30 caracteres"); }
                else
                    this.mdp = value;
            }
        }

        public Role RoleEmploye
        {
            get
            {
                return this.roleEmploye;
            }

            set
            {
                this.roleEmploye = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Employe> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Employe> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
