﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM.Classes
{
    public class Reservation : ICrud<Reservation>, INotifyPropertyChanged
    {
        private int numReservation;
        private DateTime dateReservation;
        private DateTime dateDebutLocation;
        private DateTime dateRetourEffectiveLocation;
        private DateTime? dateRetourReelleLocation;
        private decimal? prixTotal;
        private Employe employe;
        private Client client;
        private Materiel materiel;

        public Reservation()
        {
        }

        public Reservation(int numReservation, DateTime dateReservation, DateTime dateDebutLocation,
            DateTime dateRetourEffectiveLocation, DateTime? dateRetourReelleLocation,
            decimal? prixTotal, Employe employe, Client client, Materiel materiel)
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
        public Reservation(int numReservation, DateTime dateReservation, DateTime dateDebutLocation,
    DateTime dateRetourEffectiveLocation,
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
        public Reservation(DateTime dateReservation, DateTime dateDebutLocation,
DateTime dateRetourEffectiveLocation,
decimal prixTotal, Employe employe, Client client, Materiel materiel)
        {

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
                if (value.Date < DateReservation.Date)
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

        public DateTime? DateRetourReelleLocation
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

        public decimal? PrixTotal
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

        public Materiel Materiel
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
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into reservation (datereservation,datedebutlocation,dateretoureffectivelocation,numemploye,numclient,nummateriel,prixtotal) values (@datereservation,@datedebutlocation,@dateretoureffectivelocation,@numemploye,@numclient,@nummateriel,@prixtotal) RETURNING numreservation"))
            {
                cmdInsert.Parameters.AddWithValue("datereservation", this.DateReservation);
                cmdInsert.Parameters.AddWithValue("datedebutlocation", this.DateDebutLocation);
                cmdInsert.Parameters.AddWithValue("dateretoureffectivelocation", this.DateRetourEffectiveLocation);
                cmdInsert.Parameters.AddWithValue("numemploye", this.Employe.NumEmploye);
                cmdInsert.Parameters.AddWithValue("numclient", this.Client.NumClient);
                cmdInsert.Parameters.AddWithValue("nummateriel", this.Materiel.NumMateriel);
                cmdInsert.Parameters.AddWithValue("prixtotal", this.PrixTotal);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumReservation = nb;
            return nb;
        }

        public int Delete()
        {
            using (var cmdUpdate = new NpgsqlCommand("delete from reservation where numreservation =@numreservation;"))
            {
                cmdUpdate.Parameters.AddWithValue("numreservation", this.NumReservation);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public List<Reservation> FindAll(Agence agence)
        {
            List<Reservation> lesReservations = new List<Reservation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from \"main\".reservation ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    var employe = agence.Employes.SingleOrDefault(e => e.NumEmploye == (int)dr["numemploye"]);
                    var client = agence.Clients.SingleOrDefault(c => c.NumClient == (int)dr["numclient"]);
                    var materiel = agence.Materiels.SingleOrDefault(m => m.NumMateriel == (int)dr["nummateriel"]);

                    if (dr["dateretourreellelocation"] == DBNull.Value)
                    {
                        lesReservations.Add(new Reservation(
                            (int)dr["numreservation"],
                            (DateTime)dr["datereservation"],
                            (DateTime)dr["datedebutlocation"],
                            (DateTime)dr["dateretoureffectivelocation"],

                            (Decimal)dr["prixtotal"],
                            employe,
                            client,
                            materiel
                        ));
                    }
                    else
                    {
                        lesReservations.Add(new Reservation(
                            (int)dr["numreservation"],
                            (DateTime)dr["datereservation"],
                            (DateTime)dr["datedebutlocation"],
                            (DateTime)dr["dateretoureffectivelocation"],
                            (DateTime)dr["dateretourreellelocation"],
                            (Decimal)dr["prixtotal"],
                            employe,
                            client,
                            materiel
                        ));
                    }
                }


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
        public List<Reservation> FindAllAvecIdMateriel(Agence agence, int idMateriel)
        {
            List<Reservation> lesReservations = new List<Reservation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from \"main\".reservation where nummateriel =" + idMateriel + ";"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    var employe = agence.Employes.SingleOrDefault(e => e.NumEmploye == (int)dr["numemploye"]);
                    var client = agence.Clients.SingleOrDefault(c => c.NumClient == (int)dr["numclient"]);
                    var materiel = agence.Materiels.SingleOrDefault(m => m.NumMateriel == (int)dr["nummateriel"]);

                    if (dr["dateretourreellelocation"] == DBNull.Value)
                    {
                        lesReservations.Add(new Reservation(
                            (int)dr["numreservation"],
                            (DateTime)dr["datereservation"],
                            (DateTime)dr["datedebutlocation"],
                            (DateTime)dr["dateretoureffectivelocation"],

                            (Decimal)dr["prixtotal"],
                            employe,
                            client,
                            materiel
                        ));
                    }
                    else
                    {
                        lesReservations.Add(new Reservation(
                            (int)dr["numreservation"],
                            (DateTime)dr["datereservation"],
                            (DateTime)dr["datedebutlocation"],
                            (DateTime)dr["dateretoureffectivelocation"],
                            (DateTime)dr["dateretourreellelocation"],
                            (Decimal)dr["prixtotal"],
                            employe,
                            client,
                            materiel
                        ));
                    }
                }


            }
            return lesReservations;

        }
    }
}