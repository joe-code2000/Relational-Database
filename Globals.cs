using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    public static class Globals
    {
        private static List<Database> Databases = new List<Database>();

        private static List<Account> Accounts = new List<Account>();
        private static List<string> Usernames = new List<string>();
        private static ArrayList Auths = new ArrayList();

        private static Account currentAcc;

        public static void addDatabase(Database db)
        {
            Databases.Add(db);
        }

        public static void addToAccounts(Account account)
        {
            if (getAccount(account.getUsername()) == null)
            {
                Accounts.Add(account);
            }
            else
            {
                Console.WriteLine($"Can not add {account.getUsername()} because an account with this username exists");

            }
        }
        public static void createAccount(string username, string password, string role)
        {
            if (password.Length < 6)
            {
                Console.WriteLine("At least 6 characters are excepted");
                return;
            }
            addToAccounts(new Account(username, password, role));
        }
        public static Account getAccount(string username)
        {
            if (Accounts.Find(x => x.getUsername().ToLower() == username.ToLower()) != null)
            {
                return Accounts.Find(x => x.getUsername() == username);
            }
            else
            {
                if (Accounts.Count > 0)
                {
                    Console.WriteLine($"Account with the username {username} does not exists");

                }
            }
            return null;
        }
        public static Account getCurrentAccount()
        {
            if (currentAcc != null)
            {
                return currentAcc;
            }
            
            return null!;
        }
        public static void setCurrentAccount(Account account)
        {
            if (currentAcc == null)
            {
                currentAcc = account;
            }
            else
            {
                Console.WriteLine($"Please logged out of {currentAcc!.getUsername()}");
            }
        }
        public static List<Account> getAccounts()
        {
            if (Accounts.Count > 0)
            {
                return Accounts;
            }
            else
            {
                Console.WriteLine($"There are no Accounts available please create an Account");
            }
            return null!;
        }

        public static void removeFromAccounts(string username)
        {
            if (getAccount(username) != null)
            {
                Accounts.Remove(getAccount(username));
            }
            else
            {
                Console.WriteLine($"Account with {username} does not exists");
            }
        }

        //Databases
        public static Database getDatabase(string dbName)
        {
            Database db = Databases.Find(x => x.getDB() == dbName);
            return db;
        }
        public static List<Database> getDatabases()
        {
            return Databases;
        }
        public static void removeDatabase(string name)
        {
            Databases.Remove(getDatabase(name));
            Console.WriteLine($"Database {name} has been removed");
        }
        public static List<string> getUsernames()
        {
            return Usernames;
        }
        public static void addAuths(object obj)
        {
            Auths.Add(obj);
        }
    }
}
