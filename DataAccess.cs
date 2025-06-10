
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Logging;
using Npgsql;


namespace SAE_201_LOXAM
{

    public class DataAccess
    {
        private static DataAccess instance;
        private static string connectionString;
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("DataAccess non initialisé. Appeler Init d'abord.");
                }
                return instance;
            }
        }

        private DataAccess(string connString)
        {
            connectionString = connString;
            try
            {
                connection = new NpgsqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur dans le constructeur DataAccess");
                throw;
            }
        }

        public static void Init(string connString)
        {
            if (instance == null)
            {
                instance = new DataAccess(connString);
            }
            else
            {
                throw new InvalidOperationException("DataAccess déjà initialisé.");
            }
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                        throw;
                    }
                }


                return connection;
            }

            //  pour requêtes SELECT et retourne un DataTable ( table de données en mémoire)
            public DataTable ExecuteSelect(NpgsqlCommand cmd)
            {
                DataTable dataTable = new DataTable();
                try
                {
                    cmd.Connection = GetConnection();
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Erreur SQL");
                    throw;
                }
                return dataTable;
            }

            //   pour requêtes INSERT et renvoie l'ID généré

            public int ExecuteInsert(NpgsqlCommand cmd)
            {
                int nb = 0;
                try
                {
                    cmd.Connection = GetConnection();
                    nb = (int)cmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                    throw;
                }
                return nb;
            }




            //  pour requêtes UPDATE, DELETE
            public int ExecuteSet(NpgsqlCommand cmd)
            {
                int nb = 0;
                try
                {
                    cmd.Connection = GetConnection();
                    nb = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                    throw;
                }
                return nb;

            }

            // pour requêtes avec une seule valeur retour  (ex : COUNT, SUM) 
            public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
            {
                object res = null;
                try
                {
                    cmd.Connection = GetConnection();
                    res = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                    throw;
                }
                return res;

            }

            //  Fermer la connexion 
            public void CloseConnection()
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        public void EnsureConnection()
        {
            if (connection == null)
                throw new InvalidOperationException("Connexion non initialisée.");

            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
            }
        }

    }

}
