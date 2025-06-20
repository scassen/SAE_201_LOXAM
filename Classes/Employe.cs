﻿
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SAE_201_LOXAM.Classes
{
    public enum Role { Employe, ResponsableAtelier }

    public class Employe : ICrud<Employe>, INotifyPropertyChanged
    {
        private int numEmploye;
        private string nomEmploye;
        private string prenomEmploye;
        private string login;
        private string mdp;
        private Role roleEmploye;

        public Employe() { }

        public Employe(int numEmploye, string nomEmploye, string prenomEmploye, string login, string mdp, Role roleEmploye)
        {
            this.numEmploye = numEmploye;
            this.nomEmploye = nomEmploye;
            this.prenomEmploye = prenomEmploye;
            this.login = login;
            this.mdp = mdp;
            this.roleEmploye = roleEmploye;
        }

        public int NumEmploye
        {
            get
            {
                return numEmploye;
            }
            set
            {
                numEmploye = value;
            }
        }

        public string NomEmploye
        {
            get
            {
                return nomEmploye;
            }
            set
            {
                if (value.Length > 30)
                {
                    throw new ArgumentOutOfRangeException("Nom de moins de 30 caractères");
                }
                nomEmploye = value;
            }
        }

        public string PrenomEmploye
        {
            get
            {
                return prenomEmploye;
            }
            set
            {
                if (value.Length > 30)
                {
                    throw new ArgumentOutOfRangeException("Prénom de moins de 30 caractères");
                }
                prenomEmploye = value;
            }
        }

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (value.Length > 30)
                {
                    throw new ArgumentOutOfRangeException("Login de moins de 30 caractères");
                }
                login = value;
            }
        }

        public string Mdp
        {
            get
            {
                return mdp;
            }
            set
            {
                if (value.Length > 30)
                {
                    throw new ArgumentOutOfRangeException("MDP de moins de 30 caractères");
                }
                mdp = value;
            }
        }

        public Role RoleEmploye
        {
            get
            {
                return roleEmploye;
            }
            set
            {
                roleEmploye = value;
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



        public List<Employe> FindAll()
        {
            List<Employe> employes = new List<Employe>();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"main\".employe;"))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                    Console.WriteLine($"[DEBUG] Employe rows fetched: {dt.Rows.Count}");

                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            Console.WriteLine($"  Row: numemploye={dr["numemploye"]}, nom={dr["nom"]}, prenom={dr["prenom"]}");

                            employes.Add(new Employe
                            {
                                NumEmploye = Convert.ToInt32(dr["numemploye"]),
                                NomEmploye = dr["nom"].ToString(),
                                PrenomEmploye = dr["prenom"].ToString(),
                                Login = dr["login"].ToString(),
                                Mdp = dr["mdp"].ToString(),
                                RoleEmploye = Role.Employe
                            });
                        }
                        catch (Exception rowEx)
                        {
                            Console.WriteLine($"[ERROR] Skipping row due to: {rowEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Employe.FindAll failed: {ex.Message}");
            }

            return employes;
        }

        public List<Employe> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }
    }
}
