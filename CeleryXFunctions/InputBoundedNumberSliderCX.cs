using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryXFunctions
{
    [IsVisibleInDynamoLibrary(false)]
    public class InputBoundedNumberSliderCX
    {
        [IsVisibleInDynamoLibrary(false)]
        public static double GetSliderOutputValue(double leftLimitValue, double rightLimitValue, double sliderValue, int precision, int orNumberInteger)
        {
            int nprec = precision;
            if (orNumberInteger == 1)
            {
                nprec = 0;
            }

            double retval = (sliderValue * (rightLimitValue - leftLimitValue) / 100.0) + leftLimitValue;
            return Math.Round(retval, nprec);
        }
    }
}
