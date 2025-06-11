// Fichier : Materiel.cs
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SAE_201_LOXAM
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

        public int NumMateriel { get => numMateriel; set => numMateriel = value; }
        public string Reference
        {
            get => reference;
            set => reference = value.Length > 50 ? throw new ArgumentOutOfRangeException("reference") : value;
        }
        public string NomMateriel
        {
            get => nomMateriel;
            set => nomMateriel = value.Length > 100 ? throw new ArgumentOutOfRangeException("nom") : value;
        }
        public string Descriptif
        {
            get => descriptif;
            set => descriptif = value.Length > 1000 ? throw new ArgumentOutOfRangeException("descriptif") : value;
        }
        public decimal PrixJournee
        {
            get => prixJournee;
            set => prixJournee = value <= 0 ? throw new ArgumentOutOfRangeException("prix") : value;
        }
        public Etat EtatMateriel { get => etatMateriel; set => etatMateriel = value; }
        public List<Certification> Certificationsnecessaires { get => certificationsnecessaires; set => certificationsnecessaires = value; }
        internal Type TypeMateriel { get => typeMateriel; set => typeMateriel = value; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create() => throw new NotImplementedException();
        public int Delete() => throw new NotImplementedException();
        public void Read() => throw new NotImplementedException();
        public int Update() => throw new NotImplementedException();
        public List<Materiel> FindBySelection(string criteres) => throw new NotImplementedException();

        public List<Materiel> FindAll(Agence agence)
        {
            List<Materiel> lesMateriaux = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"MAIN\".materiel"))
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
                        agence.Types.SingleOrDefault(t => t.NumType == (int)dr["numtype"])
                    ));
                }
            }
            return lesMateriaux;
        }

        public List<Materiel> FindAll()
        {
            List<Materiel> lesMateriaux = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"MAIN\".materiel"))
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
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"MAIN\".necessiter"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    if ((int)dr["nummateriel"] == num)
                        certifications.Add((Certification)(int)dr["numcertification"]);
                }
            }
            return certifications;
        }
    }
}