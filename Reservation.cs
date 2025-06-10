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
    internal class Reservation:ICrud<Reservation>, INotifyPropertyChanged
    {
        private int numReservation;
        private DateTime dateReservation;
        private DateTime dateDebutLocation;
        private DateTime dateRetourEffectiveLocation;
        private DateTime dateRetourReelleLocation;
        private decimal prixTotal;
        private Employe employe;
        private Client client;
        private Materiel materiel;

        public Reservation()
        {
        }

        public Reservation(int numReservation, DateTime dateReservation, DateTime dateDebutLocation, 
            DateTime dateRetourEffectiveLocation, DateTime dateRetourReelleLocation, 
            decimal prixTotal, Employe employe, Client client, Materiel materiel)
        {
            this.NumReservation = numReservation;
            this.DateReservation = dateReservation;
            this.DateDebutLocation = dateDebutLocation;
            this.DateRetourEffectiveLocation = dateRetourEffectiveLocation;
            this.DateRetourReelleLocation = dateRetourReelleLocation;
            this.PrixTotal = prixTotal;
            this.Employe = employe;
            this.Client = client;
            this.Materiel = materiel;
        }

        public int NumReservation
        {
            get
            {
                return this.numReservation;
            }

            set
            {
                this.numReservation = value;
            }
        }

        public DateTime DateReservation
        {
            get
            {
                return this.dateReservation;
            }

            set
            {
                this.dateReservation = value;
            }
        }

        public DateTime DateDebutLocation
        {
            get
            {
                return this.dateDebutLocation;
            }

            set
            {
                if (value < DateReservation)
                { throw new ArgumentException("date de reservation doit etre avant ou au meme moment que la date de debut location"); }
                else
                this.dateDebutLocation = value;
            }
        }

        public DateTime DateRetourEffectiveLocation
        {
            get
            {
                return this.dateRetourEffectiveLocation;
            }

            set
            {
                if (value < DateDebutLocation)
                { throw new ArgumentException(""); }
                else
                this.dateRetourEffectiveLocation = value;
            }
        }

        public DateTime DateRetourReelleLocation
        {
            get
            {
                return this.dateRetourReelleLocation;
            }

            set
            {
                this.dateRetourReelleLocation = value;
            }
        }

        public decimal PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                if (value <= 0)
                { throw new ArgumentOutOfRangeException("prixtotal positif"); }
                this.prixTotal = value;
            }
        }

        public Client Client
        {
            get
            {
                return this.client;
            }

            set
            {
                this.client = value;
            }
        }

        internal Employe Employe
        {
            get
            {
                return this.employe;
            }

            set
            {
                this.employe = value;
            }
        }

        internal Materiel Materiel
        {
            get
            {
                return this.materiel;
            }

            set
            {
                this.materiel = value;
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

        public List<Reservation> FindAll(Agence agence)
        {
            List<Reservation> lesReservations = new List<Reservation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from reservations ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesReservations.Add(new Reservation((int)dr["numreservation"], (DateTime)dr["datereservation"], (DateTime)dr["datedebutlocation"], (DateTime)dr["dateretoureffectivelocation"],
                        (DateTime)dr["dateretourreellelocation"], (Decimal)dr["prixtotal"], agence.Employes.SingleOrDefault(ID => ID.NumEmploye == (int)dr["numemploye"]),
                        agence.Clients.SingleOrDefault(ID => ID.NumClient == (int)dr["numclient"]),
                        agence.Materiels.SingleOrDefault(ID => ID.NumMateriel == (int)dr["nummateriel"])));
                        
            }
            return lesReservations;
        }

        public List<Reservation> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Reservation> FindBySelection(string criteres)
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
