using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    [Serializable]
    public struct AdminUser
    {
        private string type;
        private List<string> allowedOperations;
        public void initialize()
        {
            this.type = "Admin";
            Globals.addAuths(this);
        }
    }
    [Serializable]
    public struct BasicUser
    {
        private string type;
        private List<string> allowedOperations;
        public void initialize()
        {
            this.type = "Basic";
            Globals.addAuths(this);
        }
    }
    [Serializable]
    public class Account
    {
        private string username, password;
        private string role;
        private bool loggedIn;
        private Database database;

        public Account(string username, string password, string role)
        {
            this.username = username;
            this.password = password;

            switch (role.ToLower())
            {
                case "admin":
                case "basic":
                    this.role = role;
                    break;
                default:
                    Console.WriteLine("Not a valid user type");
                    return;
            }

        }
        public string getUsername()
        {
            return this.username;
        }
        public bool login(string password)
        {
            if (this.password != password)
            {
                Console.WriteLine("Invalid login");
                return false;
            }
            Globals.setCurrentAccount(this);

            return true;
        }
        public void logout()
        {
            if (this.loggedIn)
            {
                this.loggedIn = false;
                if (Globals.getCurrentAccount() != null)
                {
                    Globals.setCurrentAccount(null);
                }

                Console.WriteLine("logged out");
            }
        }
        public Database getDatabase()
        {
            return this.database;
        }

        public void setCurrentDB(Database db)
        {
            this.database = db;
        }
        public void display()
        {
            Console.WriteLine($"Username -> {this.username}");
            if (this.loggedIn)
            {
                Console.WriteLine($"Role -> {this.role}");
            }
        }
    }
}
