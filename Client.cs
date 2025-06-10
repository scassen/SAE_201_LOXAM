using System;

namespace SAE_201_LOXAM
{
    public enum Certification
    {
        Interne = 1,
        CACES_R486 = 2,
        CACES_R482 = 3,
    }

    public class Client
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;

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
    }
}
