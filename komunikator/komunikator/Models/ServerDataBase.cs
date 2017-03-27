using komunikator.Models.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace komunikator.Models
{
    class ServerDataBase : SQLite
    {
        public void GenerateServerDataBaseForFirstTime()
        {
            SQLite database = new SQLite();
            database.Password = Properties.Settings.Default["DataBasePasswordSetting"].ToString();
            database.Create(this.DBPath, this.Name);
            database.Connect();
            //creating tables
            //table users
            string query = "CREATE TABLE USERS (ID Integer PRIMARY KEY AUTOINCREMENT, USERNAME varchar(255) NOT NULL, PASSWORD varchar(255) NOT NULL, IDNUMBER int NOT NULL)";
            database.ExecuteNonQuery(query);
            //table messages
            query = "CREATE TABLE MESSAGES (ID Integer PRIMARY KEY AUTOINCREMENT, IDNUMBERSENDER INT NOT NULL, MESSAGE varchar(255) NOT NULL, IDNUMBERRECEIVER INT NOT NULL)";
            database.ExecuteNonQuery(query);
            database.Disconnect();
            
        }

        public void ProcessMessage(string data)
        {
            char[] dataChar = data.ToCharArray();
            //message decoding
            string username = "";
            string password = "";
            for (int j = 0; j < data.Length; j++)
            {
                if (dataChar[j] == ';' && dataChar[j + 1] == '+' && dataChar[j + 2] == ';')
                {
                    break;
                }
                username = username + dataChar[j];
            }
            for (int j = (username.Length + 3); j < data.Length; j++)
            {
                if (dataChar[j] == ';' && dataChar[j + 1] == '+' && dataChar[j + 2] == ';')
                {
                    break;
                }
                password = password + dataChar[j];
            }
            
            string SentMessage = data.Substring(username.Length + password.Length + 6);
        }

        public void SendMessage(string username, string password, string message)
        {

        }

        public int GetClientID(string username, string password)
        {
            int id = 0;
            string query = "SELECT IDNUMBER FROM USERS WHERE USERNAME="+username+"AND PASSWORD="+password;
            return id;
        }
    }
}
