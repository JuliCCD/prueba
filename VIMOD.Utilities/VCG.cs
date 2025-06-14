using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Utilities
{
    public static class VCG
    {
        public static string Set(string message, string type)
        {
            return $"{type}|{message}";
        }

        public static string[] Get(string vcgMessage)
        {
            return vcgMessage.Split('|');
        }
    }
}
