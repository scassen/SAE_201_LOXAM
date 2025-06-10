using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    public enum Etat { }
    internal class Materiel:ICrud<Materiel>,INotifyPropertyChanged
    {
        private int numMateriel;
        private string reference;
        private string nomMateriel;
        private string descriptif;
        private decimal prixJournee;
        private Etat etatMateriel;
        private Type typeMateriel;
        private List<Certification> certificationsnecessaires;



        public Materiel(int numMateriel, string reference, string nomMateriel, string descriptif, decimal prixJournee, Etat etatMateriel, List<Certification> certificationsnecessaires, Type typeMateriel)
        {
            this.NumMateriel = numMateriel;
            this.Reference = reference;
            this.NomMateriel = nomMateriel;
            this.Descriptif = descriptif;
            this.PrixJournee = prixJournee;
            this.EtatMateriel = etatMateriel;
            this.Certificationsnecessaires = certificationsnecessaires;
            this.TypeMateriel = typeMateriel;
        }
        public Materiel()
        {
        }

        public int NumMateriel
        {
            get
            {
                return this.numMateriel;
            }

            set
            {
                this.numMateriel = value;
            }
        }

        public string Reference
        {
            get
            {
                return this.reference;
            }

            set
            {
                if (value.Length > 50)
                { throw new ArgumentOutOfRangeException("reference de moins de 50 caracteres"); }
                else
                    this.reference = value;
            }
        }

        public string NomMateriel
        {
            get
            {
                return this.nomMateriel;
            }

            set
            {
                if (value.Length > 100)
                { throw new ArgumentOutOfRangeException("nom de moins de 100 caracteres"); }
                else
                    this.nomMateriel = value;
            }
        }

        public string Descriptif
        {
            get
            {
                return this.descriptif;
            }

            set
            {
                if (value.Length >1000)
                { throw new ArgumentOutOfRangeException("description de moins de 1000 caracteres"); }
                else
                this.descriptif = value;
            }
        }

        public decimal PrixJournee
        {
            get
            {
                return this.prixJournee;
            }

            set
            {
                if (value <= 0)
                { throw new ArgumentOutOfRangeException("prix ne peut pas etre negatif ou egal a 0"); }
                else
                this.prixJournee = value;
            }
        }

        public Etat EtatMateriel
        {
            get
            {
                return this.etatMateriel;
            }

            set
            {
                this.etatMateriel = value;
            }
        }

        public List<Certification> Certificationsnecessaires
        {
            get
            {
                return this.certificationsnecessaires;
            }

            set
            {
                this.certificationsnecessaires = value;
            }
        }

        internal Type TypeMateriel
        {
            get
            {
                return this.typeMateriel;
            }

            set
            {
                this.typeMateriel = value;
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

        public List<Materiel> FindAll()
        {
            List<Materiel> lesMateriaux = new List<Materiel>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Materiels ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesMateriaux.Add(new Materiel());

            }
            return lesMateriaux;
        }

        public List<Materiel> FindBySelection(string criteres)
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
