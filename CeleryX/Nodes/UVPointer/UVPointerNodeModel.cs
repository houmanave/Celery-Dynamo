using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.UVPointer
{
    public class UVPointerNodeModel : NodeModel
    {


        [JsonConstructor]
        public UVPointerNodeModel(IEnumerable<PortModel> inputPorts, IEnumerable<PortModel> outputPorts) : base(inputPorts, outputPorts)
        {

        }
    }
}
