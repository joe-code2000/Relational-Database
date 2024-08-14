using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    public class Test
    {
        
        static void Main(string[] args)
        {
            Account account = new Account("Joel","joel1890","admin");

            account.login("joel1890");

            //account.logout();

            Account account2 = new Account("Eli","shitupi","basic");

            //account2.login("shitupi");

            //Account account3 = new Account("joelChits", "super");

            //account3.display();

            //foreach (Account acc in Globals.getAccounts())
            //{
            //    acc.display();
            //}



            //account2.login("shitupi");
            if (Globals.getCurrentAccount() != null)
            {
                //Console.WriteLine(Globals.currentAcc.getUsername());
            }
            else
            {
                Console.WriteLine("No user logged in");
            }
            
            Database db1 = new Database("chitalaDB",account);
            List<Column> columns = new List<Column>();
            Column col = new Column();
            List<string> attrs = new List<string>();
            attrs.Add("string");
            attrs.Add("not null");
            col.initialize("studentName",attrs);
            columns.Add(col);
            
            Column col2 = new Column();
            List<string> attrs2 = new List<string>();
            attrs2.Add("int");
            attrs2.Add("null");
            col2.initialize("Age", attrs2);
            columns.Add(col2);

            Column col3 = new Column();
            List<string> attrs3 = new List<string>();
            attrs3.Add("string");
            attrs3.Add("null");
            col3.initialize("Gender", attrs3);
            columns.Add(col3);

            db1.addTable(db1, "Students", columns);

            Table table = db1.getTables().Find(x => x.getTableName() == "Students");

            string[] comp = { "studentName", "Age", "Gender" };
            string[] inser = { "Joel", "21", "Male" };
            table.insertRow(comp, inser);

            string[] comp2 = { "studentName", "Age", "Gender" };
            string[] inser2 = { "Crystal", "21", "female" };
            table.insertRow(comp2, inser2);

            //table.dropRow(1);
            //account.logout();
            //table.dropRow(0);

            //db1.display();



            //Engine
            Engine engine = new Engine();

            string query1 = "create database myDB "+
                "create table myDB.Employees (name string, age int) " +
                "insert into Students (studentName,Age,Gender) values (Beatrice,40,Female) " +
                "select * from Students " +
                "insert into myDB.Employees (name,age) values (emp1,32) " +
                "select * from myDB.Employees";


            string query2 = "create table Students ( name string , age int , gender string)";
            //string query3 = "select * from Students where name = joel";
            //string query4 = "select name, age, gender from Students where name = joel";
            string query5 = "use chitalaDB";
            string query6 = "select studentName,Age,Gender from chitalaDB.Students where Age=21";
            string query7 = "create database myDB";
            string query8 = "drop table myDB.Employees";
            string query9 = "drop database myDB";
            string query10 = "delete row from Students where Age=20";

            string query11 = "insert into Students (studentName,Age,Gender) values (Beatrice,40,female)";
            string query12 = "select * from Students where Gender=female";
            engine.compiler(query5);



            engine.compiler(query11);

            var complie1 = engine.compiler(query12);
            complie1.rows.ForEach(row =>
            {
                Console.WriteLine($"\nindex -> {row.index}\n" +
                    $"column: {row.getColumnName()}\tvalue: {row.getValue()}");
            });

            //engine.compiler(query10);


            //string str = "order by desc";

            Globals.getDatabases().ForEach(db =>
            {
                //db.display();
            });
        }
    }
}
