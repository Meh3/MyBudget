using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Common
{
    public static class StringResource
    {
        private static ResourceManager rm = new ResourceManager("MyBudget.UI.Strings.General", Assembly.GetExecutingAssembly()) { IgnoreCase = true };

        public static string GetString(string name) => rm.GetString(name);
    }
}
