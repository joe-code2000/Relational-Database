using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    public class Client
    {
        public static void test()
        {
            Database db1 = new Database("chitalaDB", Globals.getCurrentAccount());
            List<Column> columns = new List<Column>();
            Column col = new Column();
            List<string> attrs = new List<string>();
            attrs.Add("string");
            attrs.Add("not null");
            col.initialize("studentName", attrs);
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
        }
        static void Main(string[] args)
        {
            Initialize init = new Initialize();
            if (init.isInitialized() == false)
            {
                Console.WriteLine("Run time error");
                return;
            }

            bool Process = true;

        start:
            Console.WriteLine("Select username and enter password!");
            Console.Write("-> ");
            string username = Console.ReadLine();
            Console.Write("-> ");
            string password = Console.ReadLine();

            if (Utils.isBlank(username, true) || Utils.isBlank(password, true))
            {
                Console.WriteLine("username or password fields are required");
                goto start;
            }
            Account account = Globals.getAccount(username);

            if (account == null)
            {
                Console.WriteLine("Login failed");
                goto start;
            }
            if (!account.login(password))
            {
                goto start;
            }
            if (Globals.getCurrentAccount() != null)
            {
                Console.WriteLine($"Logged in as user -> {Globals.getCurrentAccount().getUsername()}");
            }
            //test();
            Engine engine = new Engine();
            while (Process)
            {
            begin:
                Console.WriteLine("enter quit to exit, enter ; to end statement block\n");
                string execute = "";
                bool exe = false;
                while (!exe)
                {
                    Console.Write("-> ");
                    string exec = Console.ReadLine();
                    if (exec.Contains("quit") && exec.Replace(" ", "").Length == 4)
                    {
                        Console.WriteLine("Bye");
                        Globals.getDatabases().ForEach(db =>
                        {
                            fileOperations.serialize(@"Databases\" + db.getDB() + ".bin", db);
                        });
                        Process = false;
                        exe = true;
                    }
                    else
                    {
                        if (exec.Contains(";"))
                        {
                            exe = true;
                        }
                        else
                        {
                            execute += exec + " ";
                        }
                    }

                }
                try
                {
                    engine.compiler(execute).rows.ForEach(row =>
                {
                    Console.WriteLine($"\nindex -> {row.index}\n" +
                   $"column: {row.getColumnName()}\tvalue: {row.getValue()}");
                });
                    Console.WriteLine("\n" + execute + "\n");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }

            }
        }
    }
}
