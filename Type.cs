// Fichier : Type.cs
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAE_201_LOXAM
{
    public enum Categorie { Elevation = 1, EspaceVert = 2 }

    public class Type : ICrud<Type>
    {
        private int numType;
        private string libelleType;
        private Categorie categorieType;

        public Type() { }

        public Type(int numType, string libelleType, Categorie categorieType)
        {
            NumType = numType;
            LibelleType = libelleType;
            CategorieType = categorieType;
        }

        public int NumType { get => numType; set => numType = value; }
        public string LibelleType
        {
            get => libelleType;
            set => libelleType = value.Length > 30 ? throw new ArgumentOutOfRangeException("libelle") : value;
        }
        public Categorie CategorieType { get => categorieType; set => categorieType = value; }

        public int Create() => throw new NotImplementedException();
        public int Delete() => throw new NotImplementedException();
        public void Read() => throw new NotImplementedException();
        public int Update() => throw new NotImplementedException();
        public List<Type> FindBySelection(string criteres) => throw new NotImplementedException();

        public List<Type> FindAll()
        {
            List<Type> lesTypes = new();
            using (NpgsqlCommand cmdSelect = new("SELECT * FROM \"MAIN\".type"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        lesTypes.Add(new Type(
                            (int)dr["numtype"],
                            (string)dr["libelletype"],
                            (Categorie)(int)dr["numcategorie"]
                        ));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error creating Type: " + ex.Message);
                    }
                }
            }
            return lesTypes;
        }

        public static Type FindById(int numType)
        {
            using (NpgsqlCommand cmd = new("SELECT * FROM \"MAIN\".type WHERE numtype = @id"))
            {
                cmd.Parameters.AddWithValue("@id", numType);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    return new Type(
                        (int)dr["numtype"],
                        (string)dr["libelletype"],
                        (Categorie)(int)dr["numcategorie"]
                    );
                }
                throw new Exception("Type introuvable");
            }
        }
    }
}
