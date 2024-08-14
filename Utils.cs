using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database1
{
    public static class Utils
    {
        public static bool isBlank(string str, bool spaces)
        {
            if (spaces)
            {
                if (str == "" || str == " ")
                {
                    return true;
                }
            }
            else
            {
                if (str == "")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
