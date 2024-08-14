using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Database1
{
    public struct ResultSet
    {
        public int index;
        public List<columnWrapper> rows;
    }
    public struct statement
    {
        public string type;
        public List<string> metaData;
    }
    public class Engine
    {
        private List<statement> statements;
        public Engine()
        {

        }
        public List<string> parser(string query)
        {
            string[] keywords = { "create", "delete", "insert", "drop", "select", "use" };
            List<string> queries = new List<string>();
            string[] split = query.Split(" ");

            List<int> indexes = new List<int>();
            int index = 0;
            foreach (string token in split)
            {
                foreach (string keyword in keywords)
                {
                    if (token == keyword)
                    {
                        indexes.Add(index);
                    }
                }
                index++;
            }
            for (int i = 0; i < indexes.Count(); i++)
            {
                StringBuilder split_query = new StringBuilder();

                if (i + 1 == indexes.Count())
                {
                    for (int z = indexes[i]; z < split.Length; z++)
                    {
                        split_query.Append(split[z]);
                        split_query.Append(" ");
                        //Console.WriteLine(split[z]);
                    }
                }
                else
                {
                    for (int z = indexes[i]; z < indexes[i + 1]; z++)
                    {
                        split_query.Append(split[z]);
                        split_query.Append(" ");
                        //Console.WriteLine(split[z]);
                    }
                }
                queries.Add(split_query.ToString());

            }
            return queries;
        }

        public bool syntaxValidator(List<string> queries)
        {
            bool validate = true;
            this.statements = new List<statement>();

            foreach (string query in queries)
            {
                if (query.Contains(";"))
                {
                    Console.WriteLine("error invalid statement");
                    validate = false;
                    break;
                }

                bool brackets = false;
                List<int> tempIndexes = new List<int>();
                tempIndexes.Add(-1);
                List<string> indexedQuery = new List<string>();
                statement stmt = new statement();

                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i] == '(')
                    {
                        brackets = true;
                    }
                    if (query[i] == ')')
                    {
                        brackets = false;
                    }
                    if (query[i] == ' ' && !brackets)
                    {
                        tempIndexes.Add(i);
                    }
                }

                for (int i = 0; i < tempIndexes.Count(); i++)
                {
                    string str = "";
                    if (i + 1 == tempIndexes.Count())
                    {
                        for (int z = tempIndexes[i]; z < query.Length; z++)
                        {
                            str += query[z];
                            //Console.WriteLine(z);
                        }
                    }
                    else
                    {
                        for (int z = tempIndexes[i] + 1; z < tempIndexes[i + 1]; z++)
                        {
                            str += query[z];
                            //Console.WriteLine(query[z]);
                        }
                    }

                    if (str != " ")
                    {
                        if (str != "")
                        {
                            indexedQuery.Add(str);
                        }
                    }
                    stmt.type = indexedQuery[0];
                    stmt.metaData = indexedQuery;
                }
                statements.Add(stmt);
            }

            string pattern = @"([a-zA-z])";

            Regex regex = new Regex(pattern);
            bool contain;
            int bracketcount;
            string[] createKeywords = { "database", "table" };
            
            statements.ForEach(statement =>
            {
;
                switch (statement.type)
                {
                    case "create":
                        
                        if (statement.metaData[1] == "table")
                        {
                            if (statement.metaData.Count() != 4)
                            {
                                Console.WriteLine("error invalid statement");
                                Console.WriteLine(statement.type);
                                validate = false;
                                return;
                            }
                            contain = false;
                            foreach (string keyword in createKeywords)
                            {
                                if (statement.metaData.Contains(keyword))
                                {
                                    contain = true;
                                }
                            }
                            if (!contain)
                            {
                                Console.WriteLine("error invalid statement");
                                Console.WriteLine(statement.type);
                                validate = false;
                                return;
                            }

                            bracketcount = 0;

                            List<string[]> replacements = new List<string[]>();

                            statement.metaData.ForEach(meta =>
                            {
                                if (meta.Contains("("))
                                {
                                    bracketcount += 1;

                                    if (!regex.IsMatch(meta))
                                    {
                                        Console.WriteLine("error invalid statement");
                                        Console.WriteLine(statement.type);
                                        validate = false;
                                    }
                                    if (!meta.Contains(")"))
                                    {
                                        Console.WriteLine("error invalid statement");
                                        Console.WriteLine(statement.type);
                                        validate = false;
                                    }
                                    string format = "(";
                                    for (int i = 1; i < meta.Length - 1; i++)
                                    {
                                        format += meta[i];
                                        if (meta[i + 1] == ',')
                                        {
                                            format += " ";
                                        }
                                        if (meta[i] == ',')
                                        {
                                            if (meta[i + 1] != ' ')
                                            {
                                                format += " ";
                                            }
                                        }
                                    }
                                    var tokens = format.Replace("(", "").Split(" ");
                                    string format2 = "(";
                                    bool space = true;
                                    for (int i = 0; i < tokens.Length - 1; i++)
                                    {

                                        if (tokens[0] != "")
                                        {
                                            if (tokens[i] != "")
                                            {
                                                format2 += tokens[i];
                                                if (i + 2 == tokens.Length)
                                                {
                                                    format2 += " ";
                                                    format2 += tokens[i + 1];
                                                }
                                                else
                                                {
                                                    format2 += " ";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (tokens[i] != "")
                                            {
                                                space = false;
                                            }
                                            if (!space)
                                            {
                                                if (tokens[i] != "")
                                                {
                                                    format2 += tokens[i];
                                                    if (i + 2 != tokens.Length)
                                                    {
                                                        format2 += " ";
                                                    }
                                                    else
                                                    {
                                                        format2 += " ";
                                                        format2 += tokens[i + 1];
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    format2 += ")";
                                    string[] compact = { format2, $"{statement.metaData.IndexOf(meta)}" };
                                    replacements.Add(compact);
                                }
                            });

                            replacements.ForEach(rp =>
                            {
                                statement.metaData[int.Parse(rp[1])] = rp[0];
                            });

                            statement.metaData.ForEach(m =>
                            {
                                //Console.WriteLine(m);
                            });

                            if (bracketcount != 1)
                            {
                                Console.WriteLine("error invalid statement");
                                Console.WriteLine(statement.type);
                                validate = false;
                            }
                        }
                        else if (statement.metaData[1] == "database")
                        {
                            if (statement.metaData.Count() != 3)
                            {
                                Console.WriteLine("error invalid statement");
                                Console.WriteLine(statement.type);
                                validate = false;
                                return;
                            }
                        }
                        
                        
                        break;
                    case "drop":
                        
                        if (statement.metaData.Count() != 3)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }
                        if (!createKeywords.Contains(statement.metaData[1]))
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }


                        break;

                    case "delete":

                        switch (statement.metaData[1])
                        {
                            case "row":
                                if (statement.metaData.Count() != 6)
                                {
                                    Console.WriteLine("error invalid statement");
                                    Console.WriteLine(statement.type);
                                    validate = false;
                                    return;
                                }
                                switch (statement.metaData[4])
                                {
                                    case "where":
                                        if (!statement.metaData[5].Contains("="))
                                        {
                                            Console.WriteLine("error invalid statement");
                                            Console.WriteLine(statement.type);
                                            validate = false;
                                            return;
                                        }
                                        if (statement.metaData[5].Split("=")[0] == "" || statement.metaData[5].Split("=")[1] == "")
                                        {
                                            Console.WriteLine("error invalid statement");
                                            Console.WriteLine(statement.type);
                                            validate = false;
                                            return;
                                        }
                                        break;
                                    case "index":
                                        string pattern = @"(\w+\D)";
                                        Regex rg = new Regex(pattern);
                                        if (rg.IsMatch(statement.metaData[5]))
                                        {
                                            Console.WriteLine("error invalid statement");
                                            Console.WriteLine(statement.type);
                                            validate = false;
                                            return;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "rows":
                                break;
                            default:
                                break;
                        }
                        
                        
                        break;
                    case "insert":
                        string[] insertKeywords = { "into" };
                        
                        if (statement.metaData.Count() != 6)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }

                        contain = false;

                        foreach (string keyword in insertKeywords)
                        {
                            if (statement.metaData.Contains(keyword))
                            {
                                contain = true;
                            }
                        }

                        if (!contain)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }
                        
                        bracketcount = 0;

                        List<string[]> replacements_insert = new List<string[]>();

                        statement.metaData.ForEach(meta =>
                        {
                            if (meta.Contains("("))
                            {
                                bracketcount += 1;
                                if (!regex.IsMatch(meta))
                                {
                                    Console.WriteLine("error invalid statement");
                                    Console.WriteLine(statement.type);
                                    validate = false;
                                }
                                if (!meta.Contains(")"))
                                {
                                    Console.WriteLine("error invalid statement");
                                    Console.WriteLine(statement.type);
                                    validate = false;
                                }
                                string format = meta.Replace(" ", "");
                                string[] compact = { format, $"{statement.metaData.IndexOf(meta)}" };
                                replacements_insert.Add(compact);
                            }
                        });

                        replacements_insert.ForEach(rp =>
                        {
                            statement.metaData[int.Parse(rp[1])] = rp[0];
                        });

                        statement.metaData.ForEach(m =>
                        {
                            //Console.WriteLine(m);
                        });

                        if (bracketcount != 2)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                        }
                        break;
                    case "select":
                        string[] selectKeywords = { "where" };
                        List<string> meta_data = statement.metaData;

                        if (meta_data.Count() < 4)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }

                        if (!meta_data.Contains("from"))
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }

                        if ((meta_data.Contains("*") || meta_data.Contains("*,")) && meta_data.IndexOf("from") != 2)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }

                        if (meta_data.IndexOf("from") < 2)
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }

                        if (!meta_data.Contains("*"))
                        {
                            string postfix = "";
                            for (int i = 1; i < meta_data.IndexOf("from"); i++)
                            {
                                postfix += meta_data[i];

                            }
                            meta_data[1] = postfix;
                            while (meta_data[meta_data.IndexOf("from") - 1] != meta_data[meta_data.IndexOf(postfix)])
                            {
                                meta_data.Remove(meta_data[meta_data.IndexOf("from") - 1]);
                            }
                        }


                        foreach (string keyword in selectKeywords)
                        {
                            if (statement.metaData.Contains(keyword))
                            {

                                switch (keyword.ToLower())
                                {
                                    case "where":
                                        if (!meta_data[meta_data.IndexOf(keyword) + 1].Contains("="))
                                        {
                                            if (meta_data.IndexOf("=") == -1 || meta_data.IndexOf("=") + 1 == meta_data.Count())
                                            {
                                                Console.WriteLine("error invalid statement");
                                                Console.WriteLine(statement.type);
                                                validate = false;
                                                return;
                                            }

                                            string postfix = meta_data[meta_data.IndexOf("=") - 2] + " " + meta_data[meta_data.IndexOf("=") - 1] + meta_data[meta_data.IndexOf("=")] + meta_data[meta_data.IndexOf("=") + 1];

                                            meta_data[meta_data.IndexOf("=") - 1] = "";
                                            meta_data.Remove(meta_data[meta_data.IndexOf("=") - 2]);
                                            meta_data.Remove(meta_data[meta_data.IndexOf("=") - 1]);
                                            meta_data.Remove(meta_data[meta_data.IndexOf("=") + 1]);
                                            meta_data.Remove(meta_data[meta_data.IndexOf("=")]);
                                            meta_data.Add(postfix);

                                            //Console.WriteLine(postfix);

                                        }
                                        else
                                        {
                                            string postfix = keyword + " " + meta_data[meta_data.IndexOf(keyword) + 1];
                                            meta_data.Remove(meta_data[meta_data.IndexOf(keyword) + 1]);
                                            meta_data.Remove(meta_data[meta_data.IndexOf(keyword)]);
                                            meta_data.Add(postfix);
                                            //Console.WriteLine(postfix);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        meta_data.ForEach(data =>
                        {
                            //Console.WriteLine(data);
                        });
                        statement.metaData.ForEach(meta =>
                        {
                            //Console.WriteLine(meta);
                        });
                        break;
                    case "use":
                        if (statement.metaData.Count != 2 || statement.metaData[1].Contains("."))
                        {
                            Console.WriteLine("error invalid statement");
                            Console.WriteLine(statement.type);
                            validate = false;
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine("error invalid statement");
                        Console.WriteLine(statement.type);
                        validate = false;
                        return;
                }
            });
            return validate;
        }

        public ResultSet compiler(string query)
        {
            bool validated = true;
            if (Globals.getCurrentAccount() == null)
            {
                Console.WriteLine("Please login first");
                validated = false;
            }
            ResultSet rs = new ResultSet();
            rs.rows = new List<columnWrapper>();
            if (syntaxValidator(parser(query)) && validated)
            {
                this.statements.ForEach(statement =>
                {
                    switch (statement.type)
                    {
                        case "create":
                            switch (statement.metaData[1])
                            {
                                case "table":
                                    bool contains = false;
                                    if (statement.metaData[2].Contains("."))
                                    {
                                        if (Globals.getDatabase(statement.metaData[2].Split(".")[0]) != null)
                                        {
                                            Database db = Globals.getDatabase(statement.metaData[2].Split(".")[0]);
                                            contains = true;
                                            string attr = statement.metaData[3].Replace("(", "").Replace(")", "");
                                            List<Column> columns = new List<Column>();

                                            if (attr.Length > 0)
                                            {
                                                foreach (string token in attr.Split(","))
                                                {
                                                    string str = token;
                                                    Column column = new Column();
                                                    if (token[0] == ' ')
                                                    {
                                                        str = "";
                                                        for (int i = 1; i < token.Length; i++)
                                                        {
                                                            str += token[i];
                                                        }
                                                    }
                                                    List<string> attrs = new List<string>();
                                                    for (int i = 1; i < str.Split(" ").Length; i++)
                                                    {
                                                        if (str.Split(" ")[i] != "")
                                                        {
                                                            attrs.Add(str.Split(" ")[i]);
                                                        }
                                                    }
                                                    column.initialize(str.Split(" ")[0], attrs);
                                                    columns.Add(column);
                                                }
                                                db.addTable(db, statement.metaData[2].Split(".")[1], columns);
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("please select a database");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (Globals.getCurrentAccount().getDatabase() == null)
                                        {
                                            Console.WriteLine("please select a database");
                                            return;
                                        }
                                        Database db = Globals.getCurrentAccount().getDatabase();
                                        string attr = statement.metaData[3].Replace("(", "").Replace(")", "");
                                        List<Column> columns = new List<Column>();

                                        if (attr.Length > 0)
                                        {
                                            foreach (string token in attr.Split(","))
                                            {
                                                string str = token;
                                                Column column = new Column();
                                                if (token[0] == ' ')
                                                {
                                                    str = "";
                                                    for (int i = 1; i < token.Length; i++)
                                                    {
                                                        str += token[i];
                                                    }
                                                }

                                                List<string> attrs = new List<string>();
                                                for (int i = 1; i < str.Split(" ").Length; i++)
                                                {
                                                    if (str.Split(" ")[i] != "")
                                                    {
                                                        attrs.Add(str.Split(" ")[i]);
                                                    }
                                                }
                                                column.initialize(str.Split(" ")[0], attrs);
                                                columns.Add(column);
                                            }
                                            db.addTable(db, statement.metaData[2], columns);
                                        }
                                    }

                                    break;
                                case "database":
                                    Database database = new Database(statement.metaData[2],Globals.getCurrentAccount());
                                    break;
                                default:

                                    break;
                            }
                            break;
                        case "drop":
                            switch (statement.metaData[1])
                            {
                                case "table":
                                    if (statement.metaData[2].Contains("."))
                                    {
                                        
                                        Globals.getDatabase(statement.metaData[2].Split(".")[0]).dropTable(statement.metaData[2].Split(".")[1]);
                                    }
                                    else
                                    {
                                        Globals.getCurrentAccount().getDatabase().dropTable(statement.metaData[2]);
                                    }
                                    break;
                                case "database":
                                    if (statement.metaData[2].Contains("."))
                                    {
                                        Console.WriteLine("Invalid statement");
                                        return;
                                    }
                                    if (Globals.getDatabase(statement.metaData[2]) != null)
                                    {
                                        Globals.removeDatabase(statement.metaData[2]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Database not found");
                                        return;
                                    }
                                    
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case "delete":
                            switch (statement.metaData[1])
                            {
                                case "row":
                                    if (statement.metaData[3].Contains("."))
                                    {
                                        if (Globals.getDatabase(statement.metaData[3].Split(".")[0]).getTable(statement.metaData[3].Split(".")[1]) != null)
                                        {
                                            Table table_del = Globals.getDatabase(statement.metaData[3].Split(".")[0]).getTable(statement.metaData[3].Split(".")[1]);
                                            switch (statement.metaData[4])
                                            {
                                                case "where":
                                                    var rows = table_del.getRows(statement.metaData[5]);
                                                    foreach (var row in rows)
                                                    {
                                                        table_del.dropRow(row.getIndex());
                                                    }
                                                    break;
                                                case "index":
                                                    table_del.dropRow(int.Parse(statement.metaData[5]));
                                                    break;
                                                default:
                                                    Console.WriteLine("error invalid statement");
                                                    Console.WriteLine(statement.type);
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        
                                        if (Globals.getCurrentAccount().getDatabase().getTable(statement.metaData[3]) != null)
                                        {
                                            Table table_del = Globals.getCurrentAccount().getDatabase().getTable(statement.metaData[3]);
                                            switch (statement.metaData[4])
                                            {
                                                case "where":
                                                    var rows = table_del.getRows(statement.metaData[5]);
                                                    foreach (var row in rows)
                                                    {
                                                        
                                                        table_del.dropRow(row.getIndex());
                                                    }
                                                    break;
                                                case "index":
                                                    table_del.dropRow(int.Parse(statement.metaData[5]));
                                                    break;
                                                default:
                                                    Console.WriteLine("error invalid statement");
                                                    Console.WriteLine(statement.type);
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case "rows":
                                    if (statement.metaData[3].Contains("."))
                                    {
                                        if (Globals.getDatabase(statement.metaData[3].Split(".")[0]).getTable(statement.metaData[3].Split(".")[1]) != null)
                                        {
                                            Table table_del = Globals.getDatabase(statement.metaData[3].Split(".")[0]).getTable(statement.metaData[3].Split(".")[1]);
                                            table_del.dropAllRows();
                                        }
                                    }
                                    else
                                    {
                                        if (Globals.getCurrentAccount() != null)
                                        {
                                            if (Globals.getCurrentAccount().getDatabase().getTable(statement.metaData[3]) != null)
                                            {
                                                Table table_del = Globals.getCurrentAccount().getDatabase().getTable(statement.metaData[3]);
                                                table_del.dropAllRows();
                                            }
                                        }
                                        
                                    }
                                    break;
                                default:
                                    Console.WriteLine("error invalid statement");
                                    Console.WriteLine(statement.type);
                                    break;
                            }
                            break;
                        case "insert":
                            Table table;
                            if (statement.metaData[2].Contains("."))
                            {
                                if (Globals.getDatabase(statement.metaData[2].Split(".")[0]) == null)
                                {
                                    Console.WriteLine("Database not found");
                                    return;
                                }
                                table = Globals.getDatabase(statement.metaData[2].Split(".")[0]).getTables().Find(x => x.getTableName() == statement.metaData[2].Split(".")[1]);

                                if (table !=null)
                                {
                                    string[] attributes = statement.metaData[3].Replace("(", "").Replace(")", "").Split(",").ToArray();
                                    string[] values = statement.metaData[5].Replace("(", "").Replace(")", "").Split(",").ToArray();

                                    table.insertRow(attributes, values);
                                }
                                else
                                {
                                    Console.WriteLine("Table not found");
                                    return;
                                }
                            }
                            else
                            {
                                
                                if (Globals.getCurrentAccount().getDatabase() == null)
                                {
                                    Console.WriteLine("please select a database");
                                    return;
                                }
                                table = Globals.getCurrentAccount().getDatabase().getTables().Find(x => x.getTableName() == statement.metaData[2]);
                                if (table != null)
                                {
                                    string[] attributes = statement.metaData[3].Replace("(", "").Replace(")", "").Split(",").ToArray();
                                    string[] values = statement.metaData[5].Replace("(", "").Replace(")", "").Split(",").ToArray();
                                    table.insertRow(attributes, values);
                                }
                                else
                                {
                                    Console.WriteLine("Table not found");
                                    return;
                                }
                            }
                            break;
                        case "select":
                            string[] keys = { "where" };

                            if (statement.metaData[3].Contains("."))
                            {
                                if (Globals.getDatabase(statement.metaData[3].Split(".")[0]) == null)
                                {
                                    Console.WriteLine("Database not found");
                                    return;
                                }

                                table = Globals.getDatabase(statement.metaData[3].Split(".")[0]).getTables().Find(y => y.getTableName() == statement.metaData[3].Split(".")[1]);
                                if (table != null)
                                {
                                    Row[] rows = table.getRows().ToArray();
                                    if (!statement.metaData[1].Contains("*"))
                                    {
                                        string[] data = statement.metaData[1].Split(",");
                                        int start = statement.metaData.IndexOf("from") + 2;
                                        if (start != statement.metaData.Count)
                                        {
                                            for (int i = start; i < statement.metaData.Count; i++)
                                            {
                                                string aggregate = statement.metaData[i];
                                                switch (aggregate.Split(" ")[0])
                                                {
                                                    case "where":
                                                        foreach (Row row in rows)
                                                        {
                                                            var instances = row.getColumns().FindAll(x => x.getColumnName() == aggregate.Split(" ")[1].Split("=")[0] && x.getValue().ToString() == aggregate.Split("=")[1]);
                                                            if (instances.Count > 0)
                                                            {
                                                                for (int a = 0; a < instances.Count; a++)
                                                                {
                                                                    var instance = instances[a];

                                                                    row.getColumns().ForEach(col =>
                                                                    {
                                                                        for (int i = 0; i < data.Length; i++)
                                                                        {
                                                                            if (col.getColumnName() == data[i])
                                                                            {
                                                                                col.index = row.getIndex();
                                                                                rs.rows.Add(col);
                                                                            }
                                                                        }
                                                                    });

                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (Row row in rows)
                                            {
                                                for (int i = 0; i < data.Length; i++)
                                                {
                                                    var instances = row.getColumns().FindAll(x => x.getColumnName() == data[i]);
                                                    if (instances.Count > 0)
                                                    {
                                                        for (int z = 0; z < instances.Count; z++)
                                                        {
                                                            var instance = instances[z];
                                                            instance.index = row.getIndex();
                                                            rs.rows.Add(instance);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int start = statement.metaData.IndexOf("from") + 2;
                                        if (start != statement.metaData.Count)
                                        {
                                            for (int i = start; i < statement.metaData.Count; i++)
                                            {
                                                string aggregate = statement.metaData[i];
                                                switch (aggregate.Split(" ")[0])
                                                {
                                                    case "where":
                                                        foreach (Row row in rows)
                                                        {
                                                            var instances = row.getColumns().FindAll(x => x.getColumnName() == aggregate.Split(" ")[1].Split("=")[0] && x.getValue().ToString() == aggregate.Split("=")[1]);
                                                            if (instances.Count > 0)
                                                            {
                                                                for (int a = 0; a < instances.Count; a++)
                                                                {
                                                                    var instance = instances[a];

                                                                    row.getColumns().ForEach(col =>
                                                                    {

                                                                        col.index = row.getIndex();
                                                                        rs.rows.Add(col);

                                                                    });

                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (Row row in rows)
                                            {

                                                var instances = row.getColumns();
                                                if (instances.Count > 0)
                                                {
                                                    for (int z = 0; z < instances.Count; z++)
                                                    {
                                                        var instance = instances[z];
                                                        instance.index = row.getIndex();
                                                        rs.rows.Add(instance);
                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Table not found");
                                    return;
                                }
                            }
                            else
                            {
                                if (Globals.getCurrentAccount().getDatabase() == null)
                                {
                                    Console.WriteLine("Database not found");
                                    return;
                                }
                                table = Globals.getCurrentAccount().getDatabase().getTables().Find(y => y.getTableName() == statement.metaData[3]);

                                if (table != null)
                                {
                                    Row[] rows = table.getRows().ToArray();
                                    if (!statement.metaData[1].Contains("*"))
                                    {
                                        string[] data = statement.metaData[1].Split(",");
                                        int start = statement.metaData.IndexOf("from") + 2;
                                        if (start != statement.metaData.Count)
                                        {
                                            for (int i = start; i < statement.metaData.Count; i++)
                                            {
                                                string aggregate = statement.metaData[i];

                                                switch (aggregate.Split(" ")[0])
                                                {
                                                    case "where":
                                                        foreach (Row row in rows)
                                                        {
                                                            var instances = row.getColumns().FindAll(x => x.getColumnName() == aggregate.Split(" ")[1].Split("=")[0] && x.getValue().ToString() == aggregate.Split("=")[1]);
                                                            if (instances.Count > 0)
                                                            {
                                                                for (int a = 0; a < instances.Count; a++)
                                                                {
                                                                    var instance = instances[a];

                                                                    row.getColumns().ForEach(col =>
                                                                    {
                                                                        for (int i = 0; i < data.Length; i++)
                                                                        {
                                                                            if (col.getColumnName() == data[i])
                                                                            {
                                                                                col.index = row.getIndex();
                                                                                rs.rows.Add(col);
                                                                            }
                                                                        }
                                                                    });

                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            foreach (Row row in rows)
                                            {
                                                for (int i = 0; i < data.Length; i++)
                                                {
                                                    var instances = row.getColumns().FindAll(x => x.getColumnName() == data[i]);
                                                    if (instances.Count > 0)
                                                    {
                                                        for (int z = 0; z < instances.Count; z++)
                                                        {
                                                            var instance = instances[z];
                                                            instance.index = row.getIndex();
                                                            rs.rows.Add(instance);
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        int start = statement.metaData.IndexOf("from") + 2;
                                        if (start != statement.metaData.Count)
                                        {
                                            for (int i = start; i < statement.metaData.Count; i++)
                                            {
                                                string aggregate = statement.metaData[i];

                                                switch (aggregate.Split(" ")[0])
                                                {
                                                    case "where":
                                                        foreach (Row row in rows)
                                                        {
                                                            var instances = row.getColumns().FindAll(x => x.getColumnName() == aggregate.Split(" ")[1].Split("=")[0] && x.getValue().ToString() == aggregate.Split("=")[1]);
                                                            if (instances.Count > 0)
                                                            {
                                                                for (int a = 0; a < instances.Count; a++)
                                                                {
                                                                    var instance = instances[a];

                                                                    row.getColumns().ForEach(col =>
                                                                    {

                                                                        col.index = row.getIndex();
                                                                        rs.rows.Add(col);

                                                                    });

                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            foreach (Row row in rows)
                                            {

                                                var instances = row.getColumns();
                                                if (instances.Count > 0)
                                                {
                                                    for (int z = 0; z < instances.Count; z++)
                                                    {
                                                        var instance = instances[z];
                                                        instance.index = row.getIndex();
                                                        rs.rows.Add(instance);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Table not found");
                                    return;
                                }
                            
                            }

                            break;



                        case "use":
                            if (Globals.getDatabase(statement.metaData[1]) != null)
                            {
                                Globals.getCurrentAccount().setCurrentDB(Globals.getDatabase(statement.metaData[1]));
                            }
                            else
                            {
                                Console.WriteLine("Database not found");
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                });
            }

            return rs;
        }

        //public ResultSet compile(List<string> queries)
        //{
        //    ResultSet rs = new ResultSet();


        //    return rs;
        //}


    }
}
