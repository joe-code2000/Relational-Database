using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    public class Initialize
    {
        private bool initialized = false;
        public Initialize()
        {
            fileOperations.dirFiles("Databases",".bin").ForEach(file =>
            {
                Console.WriteLine(file);
                fileOperations.deserialize(file);
            });
            if (Globals.getAccount("root") == null)
            {
                Globals.createAccount("root", "root101", "admin");
            }
            
            initialized = true;
        }
        public bool isInitialized()
        {
            return this.initialized;
        }
    }
}
