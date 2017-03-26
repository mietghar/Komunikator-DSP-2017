using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komunikator.Models.Databases
{
    class SQLite : Database
    {
        #region Properties
        public override string DBType
        {
            get
            {
                return "SQLite";
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Password
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Path
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Exists
        {
            get
            {
                return File.Exists(Path+Name+".sqlite");
            }
        }

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
            if(Path==null || Path == "")
            {
                throw new Exception("Database is unknown. Give it a correct path.");
            }
            CreateSQLite(Path, Name);
        }
        
        private void CreateSQLite(string path, string name)
        {
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
                File.Create(path);
                SQLiteConnection.CreateFile(path + name + @".sqlite");
            }
        }



        public override void Connect(string name, string path, string password)
        {
            throw new NotImplementedException();
        }

       

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
