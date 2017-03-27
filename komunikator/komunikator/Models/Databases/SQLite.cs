using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;

namespace komunikator.Models.Databases
{
    class SQLite : Database
    {
        #region Properties

        public override string DBType
        {
            get
            {
                return dbType;
            }
        }
        public override string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public override string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        public override string DBPath
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }
        public bool Exists
        {
            get
            {
                return File.Exists(Path.GetFullPath(DBPath)+Name+".sqlite");
            }
        }

        #endregion

        #region Fields
        private string dbType = "SQLite";
        private string name = "";
        private string password = "";
        private string path = "";
        private SQLiteConnection m_dbConnection;
        #endregion

        #region Methods

        public override void Create(string path, string name)
        {
            CreateSQLite(path, name);
        }
        public override void Create()
        {
            if(Name==null || Name == "")
            {
                throw new Exception("Database is unknown. Give it a name.");
            }
            if(DBPath==null || DBPath == "")
            {
                throw new Exception("Database is unknown. Give it a correct path.");
            }
            CreateSQLite(DBPath, Name);
        }
        private void CreateSQLite(string path, string name)
        {
            DBPath = path;
            Name = name;
            if (Directory.Exists(path))
            {
                //checking if our database exists under specified name and path
                if(Exists)
                {
                    //throw new Exception("Error. Database with specified path and name already exists.");
                }
                else
                {
                    SQLiteConnection.CreateFile(path + name + @".sqlite");
                }
            }
            else
            {
                //File.Create(path);
                Directory.CreateDirectory(path);
                SQLiteConnection.CreateFile(path + name + @".sqlite");
            }
        }


        public override void Connect()
        {
            if(Password=="" || Password == null)
            {
                m_dbConnection = new SQLiteConnection("Data Source=" + DBPath + Name+".sqlite;Version=3;");
                m_dbConnection.Open();
            }
            else
            {
                m_dbConnection = new SQLiteConnection("Data Source="+DBPath+Name+".sqlite;Version=3;Password="+Password+";");
                m_dbConnection.Open();
            }
        }

        public void ExecuteNonQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public SQLiteDataReader ExecuteReader(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }

        public override void Disconnect()
        {
            m_dbConnection.Close();
        }

        #endregion
    }
}
