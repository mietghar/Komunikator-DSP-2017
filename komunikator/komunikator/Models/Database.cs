using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komunikator.Models
{
    public abstract class Database : IDatabase
    {
        public abstract string DBType { get; }
        public abstract string Name { get; set; }
        public abstract string Password { get; set; }
        public abstract string Path { get; set; }

        public abstract void Connect(string name, string path, string password);
        public abstract void Create(string path, string name);
        public abstract void Create();
        public abstract void Disconnect();
    }
}
