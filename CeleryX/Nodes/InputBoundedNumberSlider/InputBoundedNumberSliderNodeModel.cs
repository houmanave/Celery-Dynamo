using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryX.Nodes.InputBoundedNumberSlider
{
    [NodeName("InputBoundedNumberSlider")]
    [NodeCategory("CeleryX.UI")]
    [NodeDescription("This node contains a slider which is bounded to left and right limits coming from two inputs.")]
    [InPortDescriptions("The left limit of the bounded slider.", "The right limit of the bounded slider.")]
    [InPortNames("leftLimit", "rightLimit")]
    [InPortTypes("double", "double")]
    [OutPortDescriptions("The result of the bounded slider calculated from the slider position.")]
    [OutPortNames("result")]
    [OutPortTypes("double")]
    [IsDesignScriptCompatible]
    public class InputBoundedNumberSliderNodeModel : NodeModel
    {
        private double _leftLimitValue;
        private double _rightLimitValue;
        private double _sliderValue;
        private double _outputValue;
        private int _precision;
        private Dictionary<string, int> _dicPrecisions;
        private int _orNumberInteger;
        private bool _isNumber;

        [JsonProperty(PropertyName = "LeftLimitValue")]
        public double LeftLimitValue
        {
            get => _leftLimitValue;
            set
            {
                _leftLimitValue = value;
                RaisePropertyChanged("LeftLimitValue");
            }
        }

        [JsonProperty(PropertyName = "RightLimitValue")]
        public double RightLimitValue
        {
            get => _rightLimitValue;
            set
            {
                _rightLimitValue = value;
                RaisePropertyChanged("RightLImitValue");
            }
        }

        [JsonProperty(PropertyName = "SliderValue")]
        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                RaisePropertyChanged("SliderValue");

                UpdateOutputValue();

                OnNodeModified();
            }
        }

        [JsonProperty(PropertyName = "OutputValue")]
        public double OutputValue
        {
            get => _outputValue;
            set
            {
                _outputValue = value;
                RaisePropertyChanged("OutputValue");
            }
        }

        [JsonProperty(PropertyName = "Precision")]
        public int Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                RaisePropertyChanged("Precision");

                UpdateOutputValue();

                OnNodeModified();
            }
        }

        [JsonIgnore]
        public Dictionary<string, int> DicPrecisions
        {
            get => _dicPrecisions;
            set
            {
                _dicPrecisions = value;
                RaisePropertyChanged("DicPrecisions");
            }
        }

        [JsonProperty(PropertyName = "OrNumberInteger")]
        public int OrNumberInteger
        {
            get => _orNumberInteger;
            set
            {
                _orNumberInteger = value;
                IsNumber = value == 0;

                RaisePropertyChanged("OrNumberInteger");

                UpdateOutputValue();

                OnNodeModified();
            }
        }

        [JsonIgnore]
        public bool IsNumber
        {
            get => _isNumber;
            set
            {
                _isNumber = value;
                RaisePropertyChanged("IsNumber");
            }
        }

        [JsonConstructor]
        public InputBoundedNumberSliderNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            this.PortDisconnected += InputBoundedNumberSliderNodeModel_PortDisconnected;

            //  reload here because it does not load when there is an existing node in the script.
            DicPrecisions = new Dictionary<string, int>();
            DicPrecisions.Add("1", 1);
            DicPrecisions.Add("2", 2);
            DicPrecisions.Add("3", 3);
            DicPrecisions.Add("4", 4);
            DicPrecisions.Add("5", 5);
            DicPrecisions.Add("6", 6);
            DicPrecisions.Add("7", 7);
            DicPrecisions.Add("8", 8);

            //  Button Commands to assign here.
            //  ButtonCommand = new DelegateCommand(ShowMessage, CanShowMessage);
        }

        public InputBoundedNumberSliderNodeModel()
        {
            RegisterAllPorts();

            this.PortDisconnected += InputBoundedNumberSliderNodeModel_PortDisconnected;

            ArgumentLacing = LacingStrategy.Disabled;

            DicPrecisions = new Dictionary<string, int>();
            DicPrecisions.Add("1", 1);
            DicPrecisions.Add("2", 2);
            DicPrecisions.Add("3", 3);
            DicPrecisions.Add("4", 4);
            DicPrecisions.Add("5", 5);
            DicPrecisions.Add("6", 6);
            DicPrecisions.Add("7", 7);
            DicPrecisions.Add("8", 8);
            //DicPrecisions.Add("", );

            LeftLimitValue = 0.0;
            RightLimitValue = 100.0;
            SliderValue = 0.0;

            OrNumberInteger = 0;

            Precision = 3;
        }

        private void InputBoundedNumberSliderNodeModel_PortDisconnected(PortModel obj)
        {
            //throw new NotImplementedException();
        }

        protected override void OnBuilt()
        {
            base.OnBuilt();
            VMDataBridge.DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
        }

        public override void Dispose()
        {
            base.Dispose();
            VMDataBridge.DataBridge.Instance.UnregisterCallback(GUID.ToString());
        }

        private void DataBridgeCallback(object data)
        {
            ArrayList inputs = data as ArrayList;
            if (inputs.Count == 2)
            {
                LeftLimitValue = double.Parse(inputs[0].ToString());
                RightLimitValue = double.Parse(inputs[1].ToString());

                UpdateOutputValue();
            }
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].Connectors.Any() || !InPorts[1].Connectors.Any())
            {
                return new[]
                {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
                };
            }

            var slidervalue = AstFactory.BuildDoubleNode(SliderValue);
            var precisionvalue = AstFactory.BuildIntNode(Precision);
            var ornumberinteger = AstFactory.BuildIntNode(OrNumberInteger);
            var functioncall = AstFactory.BuildFunctionCall(
                new Func<double, double, double, int, int, double>(CeleryXFunctions.InputBoundedNumberSliderCX.GetSliderOutputValue),
                new List<AssociativeNode>
                {
                    inputAstNodes[0],
                    inputAstNodes[1],
                    slidervalue,
                    precisionvalue,
                    ornumberinteger
                });

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functioncall),
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                    VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputAstNodes))
                )
            };
        }

        private void UpdateOutputValue()
        {
            OutputValue = CeleryXFunctions.InputBoundedNumberSliderCX.GetSliderOutputValue(LeftLimitValue, RightLimitValue, SliderValue, Precision, OrNumberInteger);
        }
    }
}
