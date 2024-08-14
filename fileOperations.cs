using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Database1
{
    public static class fileOperations
    {
        public static string getPath(string file)
        {

            return Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net6.0", "") + $@"\{file}";

        }
        public static string getAbsolutePath(string file)
        {

            return file;

        }
        public static void serialize(string file, Object obj)
        {
            string path = getPath(file);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter binFormater = new BinaryFormatter();
                binFormater.Serialize(fs, obj);
            }
        }

        public static void deserialize(string file)
        {
            string path = getAbsolutePath(file);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                object obj = binaryFormatter.Deserialize(fs);
                Database DB = (Database)obj;
                Globals.addDatabase(DB);
            }
        }

        public static List<string> dirFiles(string directory, string ext)
        {
            var dir = Directory.GetFiles(Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net6.0", "") + $@"\{directory}");
            List<string> filtered = new List<string>();
            for (int i = 0; i < dir.Count(); i++)
            {
                dir[i].Split(@"\").ToList().ForEach(split =>
                {
                    if (split.Contains(ext))
                    {
                        filtered.Add(dir[i]);
                    }
                });
            }
            return filtered;
        }

    }
}
