using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    public enum Categorie {Elevation,EspaceVert}
    public class Type:ICrud<Type>
    {
        private int numType;
        private string libelleType;
        private Categorie categorieType;

        public Type()
        {
        }

        public Type(int numType, string libelleType, Categorie categorieType)
        {
            this.NumType = numType;
            this.LibelleType = libelleType;
            this.CategorieType = categorieType;
        }

        public int NumType
        {
            get
            {
                return this.numType;
            }

            set
            {
                this.numType = value;
            }
        }

        public string LibelleType
        {
            get
            {
                return this.libelleType;
            }

            set
            {
                if (value.Length > 30)
                { throw new ArgumentOutOfRangeException("libelle de moins de 30 caracteres"); }
                else
                    this.libelleType = value;
            }
        }

        public Categorie CategorieType
        {
            get
            {
                return this.categorieType;
            }

            set
            {
                this.categorieType = value;
            }
        }

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Type> FindAll()
        {
            List<Type> lesTypes = new List<Type>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from types ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesTypes.Add(new Type((Int32)dr["numtype"], (String)dr["libelletype"], (Categorie)((int)dr["numcategorie"])
                   ));
            }
            return lesTypes;
        }

        public List<Type> FindBySelection(string criteres)
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
