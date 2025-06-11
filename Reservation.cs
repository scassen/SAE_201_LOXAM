using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SAE_201_LOXAM
{
    public class Reservation : ICrud<Reservation>
    {
        public int NumReservation { get; set; }
        public DateTime DateReservation { get; set; }
        public Client Client { get; set; }
        public Materiel Materiel { get; set; }

        public Reservation() { }

        public Reservation(int numReservation, DateTime dateReservation, Client client, Materiel materiel)
        {
            NumReservation = numReservation;
            DateReservation = dateReservation;
            Client = client;
            Materiel = materiel;
        }

        public int Create()
        {
            var cmd = new NpgsqlCommand("INSERT INTO \"MAIN\".\"reservation\" (numreservation, datereservation, numclient, nummateriel) VALUES (@num, @date, @client, @materiel)");
            cmd.Parameters.AddWithValue("@num", NumReservation);
            cmd.Parameters.AddWithValue("@date", DateReservation);
            cmd.Parameters.AddWithValue("@client", Client.NumClient);
            cmd.Parameters.AddWithValue("@materiel", Materiel.NumMateriel);
            return DataAccess.Instance.ExecuteSet(cmd);
        }

        public int Delete() => throw new NotImplementedException();
        public void Read() => throw new NotImplementedException();
        public int Update() => throw new NotImplementedException();
        public List<Reservation> FindBySelection(string criteres) => throw new NotImplementedException();

        public List<Reservation> FindAll(Agence agence)
        {
            List<Reservation> reservations = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"MAIN\".\"reservation\""))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numRes = (int)dr["numreservation"];
                    DateTime date = (DateTime)dr["datereservation"];
                    int numClient = (int)dr["numclient"];
                    int numMateriel = (int)dr["nummateriel"];

                    Client client = agence.Clients.FirstOrDefault(c => c.NumClient == numClient);
                    Materiel materiel = agence.Materiels.FirstOrDefault(m => m.NumMateriel == numMateriel);


                    if (client != null && materiel != null)
                        reservations.Add(new Reservation(numRes, date, client, materiel));
                }
            }
            return reservations;
        }
        public List<Reservation> FindAll()
        {
            throw new NotImplementedException("Utilisez FindAll(Agence agence) à la place.");
        }

    }
}
