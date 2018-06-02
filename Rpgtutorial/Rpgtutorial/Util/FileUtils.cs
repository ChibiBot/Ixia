using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpgtutorial.Util
{
    class FileUtils
    {
        public static String getLoadPath(Type type) =>
            "Content/Load/" + type.ToString().Replace("Rpgtutorial.", "") + ".xml";
    }
}
