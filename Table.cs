using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    [Serializable]
    public struct typeWrapper<Tkey>
    {
        public Tkey value;
    }
    [Serializable]
    public struct columnWrapper
    {
        public int index;
        private string Name, Attribute;
        private Object Obj;

        public void initialize(string name, string attr, object obj)
        {
            this.Name = name;
            this.Attribute = attr;
            this.Obj = obj;
        }
        public string getColumnName()
        {
            return this.Name;
        }
        public object getValue()
        {
            switch (this.Attribute)
            {
                case "string":
                    var Val_str = (typeWrapper<string>)this.Obj;
                    return Val_str.value;
                    break;
                case "int":
                    typeWrapper<int> Val_int = (typeWrapper<int>)this.Obj;
                    return Val_int.value;
                    break;
                default:
                    Console.WriteLine("No value");
                    break;
            }
            return null;
        }

        public void display()
        {
            //Console.WriteLine($"Column name -> {this.Name}");

            switch (this.Attribute)
            {
                case "string":
                    var Val_str = (typeWrapper<string>)this.Obj;
                    Console.WriteLine($"Column value -> {Val_str.value}");
                    break;
                case "int":
                    typeWrapper<int> Val_int = (typeWrapper<int>)this.Obj;
                    Console.WriteLine($"Column value -> {Val_int.value}");
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public struct Column
    {
        private string Name;
        private List<string> Attributes;
        public void initialize(string name, List<string> attributes)
        {
            this.Name = name;
            this.Attributes = attributes;
        }
        public string getColumnName()
        {
            return this.Name;
        }
        public List<string> getAttrs()
        {
            return this.Attributes;
        }
        public void display()
        {
            Console.WriteLine($"\nColumn Name: {this.Name}");
            Console.WriteLine("Column Attributes:");
            foreach (string attribute in this.Attributes)
            {
                Console.WriteLine($"-> {attribute}");
            }
        }
    }
    [Serializable]
    public struct Row
    {
        private int index;
        private List<columnWrapper> colWrappers;

        public void initialize(int index, List<columnWrapper> columns)
        {
            this.index = index;
            this.colWrappers = columns;
        }
        public int getIndex()
        {
            return this.index;
        }
        public List<columnWrapper> getColumns()
        {
            return this.colWrappers;
        }
        public void display()
        {
            Console.WriteLine($"\nIndex -> {this.index}");
            foreach (columnWrapper colWrapper in colWrappers)
            {
                colWrapper.display();
            }
        }
    }
    [Serializable]
    public class Table
    {
        private string Name;
        private Database db;
        private List<Column> columns = new List<Column>();
        private List<Row> rows = new List<Row>();

        public Table(Database db, string name, List<Column> columns)
        {
            this.db = db;
            this.Name = name;

            foreach (Column column in columns)
            {
                this.columns.Add(column);
            }

        }
        public void dropRow(int index)
        {
            if (this.db.getOwner().getUsername() != Globals.getCurrentAccount().getUsername())
            {
                Console.WriteLine("Not permitted");
                return;
            }
            var rowList = this.rows.FindAll(x => x.getIndex() == this.getRow(index).getIndex());
            
            if (rowList.Count > 0)
            {
                this.rows.Remove(this.getRow(index));
            }
            else
            {
                Console.WriteLine($"Row with index -> {index} does not exist");
            }
        }
        public void dropAllRows()
        {
            if (this.db.getOwner().getUsername() != Globals.getCurrentAccount().getUsername())
            {
                Console.WriteLine("Not permitted");
                return;
            }
            this.rows = new List<Row>();
        }
        public void insertRow(string[] colsCompare, string[] colsInser)
        {
            
            if (this.db.getOwner().getUsername() != Globals.getCurrentAccount().getUsername())
            {
                Console.WriteLine("Not permitted");
                return;
            }
            Row row = new Row();
            int index;
            if (rows.Count() == 0)
            {
                index = 0;
            }
            else
            {
                index = rows[rows.Count() - 1].getIndex() + 1;
            }
            if (colsCompare.Length != colsInser.Length)
            {
                Console.WriteLine("Error");
                return;
            }
            Column[] cols = this.columns.ToArray();
            string[] colsName = this.columns.Select(s => s.getColumnName()).ToArray();
            List<columnWrapper> wrappers = new List<columnWrapper>();
            foreach (string colCompare in colsCompare)
            {
                if (!colsName.Contains(colCompare))
                {
                    Console.WriteLine($"{colCompare} -> Not a column of this table");
                    return;
                }
                var column = this.columns.Find(x => x.getColumnName() == colCompare);
                string attr = column.getAttrs()[0];

                columnWrapper colWrapper = new columnWrapper();
                switch (attr)
                {
                    case "string":
                        typeWrapper<string> val_str = new typeWrapper<string>();
                        val_str.value = colsInser[colsCompare.ToList().IndexOf(colCompare)];
                        colWrapper.initialize(colCompare, attr, val_str);
                        wrappers.Add(colWrapper);
                        break;
                    case "int":
                        typeWrapper<int> val_int = new typeWrapper<int>();
                        val_int.value = int.Parse(colsInser[colsCompare.ToList().IndexOf(colCompare)]);
                        colWrapper.initialize(colCompare, attr, val_int);
                        wrappers.Add(colWrapper);
                        break;
                    default:
                        Console.WriteLine("Data type not supported");
                        return;
                }
            }
            row.initialize(index, wrappers);
            this.rows.Add(row);
            
        }
        public string getTableName()
        {
            return this.Name;
        }
        private Row getRow(int index)
        {
            if (this.db.getOwner().getUsername() != Globals.getCurrentAccount().getUsername())
            {
                Console.WriteLine("Not permitted");
            }
            return this.rows.Find(x => x.getIndex() == index);
        }
        public List<Row> getRows()
        {
            return this.rows;
        }
        public List<Row> getRows(string paramater)
        {
            List<Row> rowList = new List<Row>();
            for (int i = 0; i < this.rows.Count; i++)
            {
                Row row = this.rows[i];
                for (int j = 0; j < row.getColumns().Count; j++)
                {
                    columnWrapper column = row.getColumns()[j];
                    if (column.getColumnName() == paramater.Split("=")[0] && column.getValue().ToString() == paramater.Split("=")[1].ToString())
                    {
                        rowList.Add(row);
                    }
                }
            }
            
            return rowList;
        }

        public void display()
        {
            Console.WriteLine($"\nDatabase Name: {this.db.getDB()}\tTable Name: {this.Name}");
            foreach (Column column in columns)
            {
                column.display();
            }
            foreach (Row row in rows)
            {
                row.display();
            }
        }
    }
}
