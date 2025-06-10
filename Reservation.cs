using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    internal class Reservation
    {
        private int numReservation;
        private DateOnly dateReservation;
        private DateOnly dateDebutLocation;
        private DateOnly dateRetourEffectiveLocation;
        private DateOnly dateRetourReelleLocation;
        private decimal prixTotal;
        private Employe employe;
        private Client client;
        private Materiel materiel;

        public Reservation(int numReservation, DateOnly dateReservation, DateOnly dateDebutLocation, 
            DateOnly dateRetourEffectiveLocation, DateOnly dateRetourReelleLocation, 
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

        public DateOnly DateReservation
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

        public DateOnly DateDebutLocation
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

        public DateOnly DateRetourEffectiveLocation
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

        public DateOnly DateRetourReelleLocation
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
    }
}
