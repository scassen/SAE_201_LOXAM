// Fichier : Client.cs
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SAE_201_LOXAM.Classes
{
    public enum Certification
    {
        Interne = 1,
        CACES_R486 = 2,
        CACES_R482 = 3,
    }

    public class Client : ICrud<Client>, INotifyPropertyChanged
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private List<Certification> certifications;

        public Client() { }

        public Client(int numClient, string nomClient, string prenomClient, List<Certification> certifications)
        {
            NumClient = numClient;
            NomClient = nomClient;
            PrenomClient = prenomClient;
            Certifications = certifications;
        }

        public int NumClient { get => numClient; set => numClient = value; }
        public string NomClient
        {
            get
            {
                return nomClient;
            }
            set
            {
                if (value.Length > 30) throw new ArgumentException("Le nom du client ne peut pas dépasser 30 caractères.");
                nomClient = value;
            }
        }

        public string PrenomClient
        {
            get
            {
                return prenomClient;
            }
            set
            {
                if (value.Length > 30) throw new ArgumentException("Le prénom du client ne peut pas dépasser 30 caractères.");
                prenomClient = value;
            }
        }

        public List<Certification> Certifications
        {
            get
            {
                return certifications;
            }
            set
            {
                certifications = value;
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

        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public List<Client> FindAll()
        {
            List<Client> lesClients = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"main\".client;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int num = (int)dr["numclient"];
                    lesClients.Add(new Client(num, (string)dr["nomclient"], (string)dr["prenomclient"], FindAllCertifications(num)));
                }
            }
            return lesClients;
        }

        private List<Certification> FindAllCertifications(int num)
        {
            List<Certification> certifications = new();
            using (NpgsqlCommand cmdSelect = new("select * from \"main\".dispose n join \"main\".client c on c.numclient = n.numclient group by n.numclient,n.numcertification,c.numclient having n.numclient =" + num))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {

                    certifications.Add((Certification)(int)dr["numcertification"]);
                }
            }
            return certifications;
        }
    }
}
