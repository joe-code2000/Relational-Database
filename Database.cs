using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    [Serializable]
    public class Database
    {
        private string Name;
        private Account Owner;
        private List<Table> tables = new List<Table>();

        public Database(string name,Account owner)
        {
            Globals.getDatabases().ForEach(db =>
            {
                if (db.getDB() == name)
                {
                    Console.WriteLine("Database with this name exists");
                    return;
                }
            });
            this.Name = name;
            this.Owner = owner;
            Globals.addDatabase(this);
        }

        public void addTable(Database db,string tableName, List<Column> columns)
        {
            foreach (Table table in tables)
            {
                if (table.getTableName().ToLower() == tableName.ToLower())
                {
                    Console.WriteLine($"Table {tableName} already exists please a different table");
                    return;
                }
            }
            tables.Add(new Table(db,tableName,columns));
        }
        public void dropTable(string tableName)
        {
            if (this.tables.Find(x => x.getTableName() == tableName) != null)
            {
                this.tables.Remove(this.tables.Find(x => x.getTableName() == tableName));

            }
            else
            {
                Console.WriteLine($"Table {tableName} is not found in Database {this.Name}");
                return;
            }
        }
        public string getDB()
        {
            return this.Name;
        }
        public Table getTable(string tableName)
        {
            if (this.tables.Find(x => x.getTableName() == tableName) != null)
            {
                return this.tables.Find(x => x.getTableName() == tableName);
            }
            else
            {
                Console.WriteLine($"Table {tableName} is not found in Database {this.Name}");
            }
            return null;
        }
        public List<Table> getTables()
        {
            return tables;
        }
        public Account getOwner()
        {
            return this.Owner;
        }
        public void display()
        {
            Console.WriteLine($"\nDatabase Name: {this.Name}\n" +
                $"Database Owner: {this.Owner.getUsername()}\n");
            foreach (Table table in this.tables)
            {
                table.display();
            }
        }
    }
}
