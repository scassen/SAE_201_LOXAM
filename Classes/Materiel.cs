// Fichier : Materiel.cs
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SAE_201_LOXAM.Classes
{
    public enum Etat
    {
        Disponible = 1,
        EnMaintenance = 2,
        Loue = 3
    }

    public class Materiel : ICrud<Materiel>, INotifyPropertyChanged
    {
        private int numMateriel;
        private string reference;
        private string nomMateriel;
        private string descriptif;
        private decimal prixJournee;
        private Etat etatMateriel;
        private Type typeMateriel;
        private List<Certification> certificationsnecessaires;

        public Materiel() { }

        public Materiel(int numMateriel, string reference, string nomMateriel, string descriptif, decimal prixJournee,
                        Etat etatMateriel, List<Certification> certificationsnecessaires, Type typeMateriel)
        {
            NumMateriel = numMateriel;
            Reference = reference;
            NomMateriel = nomMateriel;
            Descriptif = descriptif;
            PrixJournee = prixJournee;
            EtatMateriel = etatMateriel;
            Certificationsnecessaires = certificationsnecessaires;
            TypeMateriel = typeMateriel;
        }

        public int NumMateriel
        {
            get
            {
                return numMateriel;
            }
            set
            {
                numMateriel = value;
            }
        }

        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                if (value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("reference");
                }
                reference = value;
            }
        }

        public string NomMateriel
        {
            get
            {
                return nomMateriel;
            }
            set
            {
                if (value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("nom");
                }
                nomMateriel = value;
            }
        }

        public string Descriptif
        {
            get
            {
                return descriptif;
            }
            set
            {
                if (value.Length > 1000)
                {
                    throw new ArgumentOutOfRangeException("descriptif");
                }
                descriptif = value;
            }
        }

        public decimal PrixJournee
        {
            get
            {
                return prixJournee;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("prix");
                }
                prixJournee = value;
            }
        }

        public Etat EtatMateriel
        {
            get
            {
                return etatMateriel;
            }
            set
            {
                etatMateriel = value;
            }
        }

        public List<Certification> Certificationsnecessaires
        {
            get
            {
                return certificationsnecessaires;
            }
            set
            {
                certificationsnecessaires = value;
            }
        }

        public Type TypeMateriel
        {
            get
            {
                return typeMateriel;
            }
            set
            {
                typeMateriel = value;
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

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        public List<Materiel> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }


        public List<Materiel> FindAll(Agence agence)
        {
            List<Materiel> lesMateriaux = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"main\".materiel"))
            {

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    var type = agence.Types.SingleOrDefault(t => t.NumType == (int)dr["numtype"]);
                    if (type == null)
                        throw new Exception($"Type not found for NumType = {(int)dr["numtype"]}");
                    lesMateriaux.Add(new Materiel(
                        (int)dr["nummateriel"],
                        (string)dr["reference"],
                        (string)dr["nommateriel"],
                        (string)dr["descriptif"],
                        (decimal)dr["prixjournee"],
                        (Etat)(int)dr["numetat"],
                        FindAllCertifications((int)dr["nummateriel"]),
                        type));

                }
            }
            return lesMateriaux;
        }

        public List<Materiel> FindAll()
        {
            List<Materiel> lesMateriaux = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"main\".materiel"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesMateriaux.Add(new Materiel(
                        (int)dr["nummateriel"],
                        (string)dr["reference"],
                        (string)dr["nommateriel"],
                        (string)dr["descriptif"],
                        (decimal)dr["prixjournee"],
                        (Etat)(int)dr["numetat"],
                        FindAllCertifications((int)dr["nummateriel"]),
                        Type.FindById((int)dr["numtype"])
                    ));
                }
            }
            return lesMateriaux;
        }

        private List<Certification> FindAllCertifications(int num)
        {
            List<Certification> certifications = new();
            using (NpgsqlCommand cmdSelect = new("select * from \"main\".necessiter n join \"main\".materiel m on n.nummateriel = m.nummateriel group by n.numcertification,m.nummateriel,n.nummateriel having m.nummateriel =" + num))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {

                    certifications.Add((Certification)(int)dr["numcertification"]);
                }
            }
            return certifications;
        }

        public void UpdateEtatEnMaintenance()
        {

            var cmd = new NpgsqlCommand("UPDATE \"main\".materiel SET numetat = @numetat WHERE nummateriel = @nummateriel");

            if (this.EtatMateriel == Etat.EnMaintenance)
            {
                cmd.Parameters.AddWithValue("@numetat", 1);
                cmd.Parameters.AddWithValue("@nummateriel", this.NumMateriel);
            }

            else
            {
                throw new Exception("Ce materiel n'est déjà plus en maintenance");
            }


            DataAccess.Instance.ExecuteSet(cmd);
        }
        public void UpdateEtatDispo()
        {

            var cmd = new NpgsqlCommand("UPDATE \"main\".materiel SET numetat = @numetat WHERE nummateriel = @nummateriel");

            if (this.EtatMateriel == Etat.Disponible)
            {
                cmd.Parameters.AddWithValue("@numetat", 2);
                cmd.Parameters.AddWithValue("@nummateriel", this.NumMateriel);
            }

            else
            {
                throw new Exception("Ce materiel n'est déjà plus en réparation");
            }


            DataAccess.Instance.ExecuteSet(cmd);
        }



        public string CertificationsDisplay
        {
            get
            {
                if (Certificationsnecessaires == null || Certificationsnecessaires.Count == 0)
                    return "";
                return string.Join(", ", Certificationsnecessaires.Select(c => c.ToString()));
            }
        }
        public bool EstDisponible(DateTime dateDebut, DateTime dateFin, ObservableCollection<Reservation> lesReservations)
        {

            foreach (Reservation reservation in lesReservations)
            {
                if (reservation.Materiel.numMateriel == this.NumMateriel)
                {
                    if (!(dateFin < reservation.DateDebutLocation || dateDebut > reservation.DateRetourReelleLocation))
                    {
                        return false;
                    }

                }

            }
            return true;
        }
    }
}