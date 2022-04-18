using CeleryX.CommonControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CeleryX.Converters
{
    public class StringToPointThumbConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(CxControlPointFree) ||
                objectType == typeof(CxControlPointOrtho) /*||
                objectType == typeof(cecoPointOrtho) ||
                objectType == typeof(cecoPointControl)*/);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string str = reader.Value as string;
            string[] pointstr = str.Split(',');

            if (objectType == typeof(CxControlPointFree))
            {
                return new CxControlPointFree(new System.Windows.Point(
                    double.Parse(pointstr[0]),
                    double.Parse(pointstr[1])
                    ),
                    double.Parse(pointstr[2]),
                    double.Parse(pointstr[3]));
            }
            else if (objectType == typeof(CxControlPointOrtho))
            {
                return new CxControlPointOrtho(new System.Windows.Point(
                    double.Parse(pointstr[0]),
                    double.Parse(pointstr[1])
                    ),
                    bool.Parse(pointstr[2]),
                    double.Parse(pointstr[3]),
                    double.Parse(pointstr[4])
                    );
            }
            //else
            //if (objectType == typeof(cecoPointOrtho))
            //{
            //    return new cecoPointOrtho(new System.Windows.Point(
            //        double.Parse(pointstr[0]),
            //        double.Parse(pointstr[1])
            //        ),
            //        double.Parse(pointstr[2]),
            //        double.Parse(pointstr[3]),
            //        bool.Parse(pointstr[4])
            //        );
            //}
            //else
            //if (objectType == typeof(cecoPointControl))
            //{
            //    return new cecoPointControl(new System.Windows.Point(
            //        double.Parse(pointstr[0]),
            //        double.Parse(pointstr[1])
            //        ));
            //}
            else
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
