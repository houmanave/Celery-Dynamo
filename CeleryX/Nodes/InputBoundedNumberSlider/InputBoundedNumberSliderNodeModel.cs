using Dynamo.Engine;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        internal EngineController EngineController { get; set; }

        [JsonProperty(PropertyName = "LeftLimitValue")]
        public double LeftLimitValue
        {
            get => _leftLimitValue;
            set
            {
                _leftLimitValue = value;
                RaisePropertyChanged("LeftLimitValue");

                //UpdateOutputValue();

                //  this function calls BuildOutputAST.
                //OnNodeModified();
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

                //UpdateOutputValue();

                //  this function calls BuildOutputAST.
                //OnNodeModified();
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

                //  this function calls BuildOutputAST.
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

                //  this function calls BuildOutputAST.
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

                //  this function calls BuildOutputAST.
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

        [JsonIgnore]
        public bool IsUntriggerComputeOutput { get; private set; }

        public event Action RequestChangeVariableNumberSlider;

        protected virtual void OnRequestsChangeVariableNumberSlider()
        {
            RequestChangeVariableNumberSlider?.Invoke();
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

            //
            foreach (var port in InPorts)
            {
                port.Connectors.CollectionChanged += ConnectorsCollectionChanged;
            }
            this.PropertyChanged += IBNSPropertyChanged;
            //

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

        private void IBNSPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (IsUntriggerComputeOutput)
            //{
            //    //
            //    //Debug.WriteLine("_propertychanged");
            //    //

            //    IsUntriggerComputeOutput = false;
            //    return;
            //}

            if (e.PropertyName != "CachedValue")
            {
                return;
            }
            if (InPorts.Any(x => x.Connectors.Count == 0))
            {
                return;
            }

            //  the function of this is to update the UI whenever there is a
            //  change of tne value of either limits.
            //  this connects to the UpdateVariaNumberSlider from the NodeViewCustomization
            OnRequestsChangeVariableNumberSlider();
        }

        private void ConnectorsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //  the function of this is to update the UI whenever there is a
            //  change of tne value of either limits.
            //  this connects to the UpdateVariaNumberSlider from the NodeViewCustomization
            OnRequestsChangeVariableNumberSlider();
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

        /// <summary>
        /// Callback method for DataBridge mechanism.
        /// This callback only gets called when 
        ///     - The AST is executed
        ///     - After the BuildOutputAST function is executed 
        ///     - The AST is fully built
        /// </summary>
        private void DataBridgeCallback(object data)
        {
            ArrayList inputs = data as ArrayList;
            if (inputs.Count == 2)
            {
                LeftLimitValue = double.Parse(inputs[0].ToString());
                RightLimitValue = double.Parse(inputs[1].ToString());

                UpdateOutputValue();
            }

            RaisePropertyChanged("DataUpdated");
        }

        public void ComputeOutput(EngineController engine)
        {
            GetLeftAndRightLimitValues(engine);

            if (InPorts[0].Connectors.Any() && InPorts[1].Connectors.Any())
            {
                UpdateOutputValue();

                IsUntriggerComputeOutput = true;
            }
        }

        private void GetLeftAndRightLimitValues(EngineController engine)
        {
            if (InPorts[0].Connectors.Any() && InPorts[1].Connectors.Any())
            {
                var minnode = InPorts[0].Connectors[0].Start.Owner;
                var minindex = InPorts[0].Connectors[0].Start.Index;
                var maxnode = InPorts[1].Connectors[0].Start.Owner;
                var maxindex = InPorts[1].Connectors[0].Start.Index;

                var minid = minnode.GetAstIdentifierForOutputIndex(minindex).Name;
                var maxid = maxnode.GetAstIdentifierForOutputIndex(maxindex).Name;

                var minmirror = engine.GetMirror(minid);
                var maxmirror = engine.GetMirror(maxid);

                object mino = null;
                object maxo = null;

                if (minmirror.GetData().IsCollection)
                {
                    mino = minmirror.GetData().GetElements().Select(x => x.Data).FirstOrDefault();
                }
                else
                {
                    mino = minmirror.GetData().Data;
                }

                if (maxmirror.GetData().IsCollection)
                {
                    maxo = maxmirror.GetData().GetElements().Select(x => x.Data).FirstOrDefault();
                }
                else
                {
                    maxo = maxmirror.GetData().Data;
                }

                LeftLimitValue = TryConvertToDouble(mino, out double parsed0) ? parsed0 : 0.0;
                RightLimitValue = TryConvertToDouble(maxo, out double parsed1) ? parsed1 : 0.0;
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
            else if (EngineController != null)
            {
                GetLeftAndRightLimitValues(EngineController);
            }

            var inputval0 = inputAstNodes[0];
            var inputval1 = inputAstNodes[1];
            var sliderval = AstFactory.BuildDoubleNode(SliderValue);
            var dprecis = AstFactory.BuildIntNode(Precision);
            var dornumint = AstFactory.BuildIntNode(OrNumberInteger);

            List<AssociativeNode> inputNodes = new List<AssociativeNode>
            {
                inputval0,
                inputval1,
                sliderval,
                dprecis,
                dornumint
            };

            var functioncall = AstFactory.BuildFunctionCall(
                new Func<double, double, double, int, int, double>(CeleryXFunctions.InputBoundedNumberSliderCX.GetSliderOutputValue),
                inputNodes);

            var outputvaluenode = AstFactory.BuildDoubleNode(OutputValue);

            return new[]
            {
                //AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), outputvaluenode),
                //AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functioncall),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functioncall),
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"), VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputNodes))
                    )
            };
        }

        private void UpdateOutputValue()
        {
            OutputValue = CeleryXFunctions.InputBoundedNumberSliderCX.GetSliderOutputValue(LeftLimitValue, RightLimitValue, SliderValue, Precision, OrNumberInteger);
        }

        private static bool TryConvertToDouble(object value, out double parsed)
        {
            parsed = default(double);

            try
            {
                parsed = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
