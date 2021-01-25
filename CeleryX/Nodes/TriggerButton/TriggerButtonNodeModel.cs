using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CeleryX.Nodes.TriggerButton
{
    [NodeName("TriggerButton")]
    [NodeDescription("Passes a temporary true value to execute nodes that can be triggered externally.")]
    [NodeCategory("CeleryX.UI")]
    [OutPortNames("tf")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("The boolean value.")]
    [IsDesignScriptCompatible]
    public class TriggerButtonNodeModel : NodeModel
    {
        private bool _triggerValue;

        public bool TriggerValue
        {
            get { return _triggerValue; }
            set {
                _triggerValue = value;
                RaisePropertyChanged("TriggerValue");
                OnNodeModified(false);
            }
        }

        [JsonConstructor]
        public TriggerButtonNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            PortDisconnected += TriggerButtonNodeModelPortDisconnected;
        }

        private void TriggerButtonNodeModelPortDisconnected(PortModel obj)
        {
        }

        public TriggerButtonNodeModel()
        {
            RegisterAllPorts();

            PortDisconnected += TriggerButtonNodeModelPortDisconnected;

            _triggerValue = false;
            //ButtonCommand = new DelegateCommand(TriggerTheButton, CanShowTriggerTheButton);
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var boolnode = AstFactory.BuildBooleanNode(TriggerValue);

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), boolnode)
            };
        }

        private void TriggerTheButton()
        {

        }

        private bool CanShowTriggerTheButton()
        {
            return true;
        }
    }
}
