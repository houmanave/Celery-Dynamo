using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryXFunctions
{
    [IsVisibleInDynamoLibrary(false)]
    public class UVPointerCX
    {

        [IsVisibleInDynamoLibrary(false)]
        public static double GetUVPointerOutputValue(double u, double v)
        {
            return v;
        }
    }
}
