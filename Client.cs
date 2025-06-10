using System;
using System.ComponentModel;

namespace SAE_201_LOXAM
{
    public enum Certification
    {
        Interne = 1,
        CACES_R486 = 2,
        CACES_R482 = 3,
    }

    public class Client:ICrud<Client>,INotifyPropertyChanged
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private List<Certification> certifications;

        public Client()
        {
        }

        public Client(int numClient, string nomClient, string prenomClient, List<Certification> certifications)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.Certifications = certifications;
         
        }

        public int NumClient
        {
            get { return this.numClient; }
            set { this.numClient = value; }
        }

        public string NomClient
        {
            get { return this.nomClient; }
            set
            {
                if (value.Length > 30)
                    throw new ArgumentException("Le nom du client ne peut pas dépasser 30 caractères.");
                this.nomClient = value;
            }
        }

        public string PrenomClient
        {
            get { return this.prenomClient; }
            set
            {
                if (value.Length > 30)
                    throw new ArgumentException("Le prénom du client ne peut pas dépasser 30 caractères.");
                this.prenomClient = value;
            }
        }

        public List<Certification> Certifications
        {
            get
            {
                return this.certifications;
            }

            set
            {
                this.certifications = value;
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

        public List<Client> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Client> FindBySelection(string criteres)
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
