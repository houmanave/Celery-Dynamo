using Autodesk.DesignScript.Runtime;
using Dynamo.Engine;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.Valve
{
    [NodeName("Valve")]
    [NodeCategory("CeleryX.UI")]
    [NodeDescription("This node triggers an input to pass through the output depending on the selected switch.")]
    [InPortDescriptions("The value to pass.")]
    [InPortNames("data")]
    [InPortTypes("object")]
    [OutPortDescriptions("The value that passes.")]
    [OutPortNames("data")]
    [OutPortTypes("object")]
    [IsDesignScriptCompatible]
    public class ValveNodeModel : NodeModel
    {
        private int _isOpenClose;

        //[JsonProperty(PropertyName = "IsOpenClose")]
        public int IsOpenClose {
            get => _isOpenClose;
            set
            {
                _isOpenClose = value;
                RaisePropertyChanged("IsOpenClose");
                OnNodeModified(false);
            }
        }

        [JsonConstructor]
        public ValveNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            PortDisconnected += ValveNodeModelPortDisconnected;
        }

        public ValveNodeModel()
        {
            RegisterAllPorts();

            PortDisconnected += ValveNodeModelPortDisconnected;

            _isOpenClose = 0;
        }

        private void ValveNodeModelPortDisconnected(PortModel obj)
        {
        }

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].Connectors.Any())
            {
                return new[]
                {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
                };
            }

            var outputvaluenode = IsOpenClose == 1 ? inputAstNodes[0] : AstFactory.BuildNullNode();

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), outputvaluenode),
            };
        }
    }
}
