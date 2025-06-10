using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_201_LOXAM
{
    public enum Categorie {Elevation,EspaceVert}
    internal class Type
    {
        private int numType;
        private string libelleType;
        private Categorie categorieType;

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
    }
}
