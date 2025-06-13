
using System;
using System.Data;
using Npgsql;

namespace SAE_201_LOXAM.Classes
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
                    throw new InvalidOperationException("DataAccess non initialisé. Appeler Init d'abord.");
                return instance;
            }
        }

        private DataAccess(string connString)
        {
            connectionString = connString;
            connection = new NpgsqlConnection(connectionString);
        }

        public static void Init(string connString)
        {
            if (instance == null)
                instance = new DataAccess(connString);
            else
                throw new InvalidOperationException("DataAccess déjà initialisé.");
        }

        public NpgsqlConnection GetConnection()
        {
            if (connection == null)
                connection = new NpgsqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                connection.Open();
            return connection;
        }

        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            DataTable dt = new();
            using (var adapter = new NpgsqlDataAdapter(cmd))
                adapter.Fill(dt);
            return dt;
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            return (int)cmd.ExecuteScalar();
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            return cmd.ExecuteScalar();
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
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
