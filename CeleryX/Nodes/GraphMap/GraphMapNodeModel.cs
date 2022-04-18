using Autodesk.DesignScript.Runtime;
using CeleryX.CommonControls;
using CeleryX.CommonControls.Subcontrols;
using CeleryX.Converters;
using Dynamo.Engine;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CeleryX.Nodes.GraphMap
{
    [NodeName("GraphMap")]
    [NodeCategory("CeleryX.UI")]
    [NodeDescription("A graphical interface node to generate a collection of numbers out of lower limit, upper limit, and selected graph.")]
    [InPortNames("lowerLimit", "upperLimit", "count")]
    [InPortTypes("double", "double", "int")]
    [InPortDescriptions("The lower limit of the number collection.", "The upper limit of the number collection.", "The quantity of numbers in the collection")]
    [OutPortTypes("List<double>")]
    [OutPortNames("numColl")]
    [OutPortDescriptions("The generated collection of numbers.")]
    [IsDesignScriptCompatible]
    public class GraphMapNodeModel : NodeModel
    {
        [JsonIgnore]
        internal EngineController EngineController { get; set; }

        private List<double> listNumbers;
        private double lowerLimit;
        private double upperLimit;
        private int count;

        [JsonProperty(PropertyName = "Count")]
        public int Count
        {
            get => count;
            set { count = value; }
        }

        [JsonProperty(PropertyName = "UpperLimit")]
        public double UpperLimit
        {
            get => upperLimit;
            set {
                upperLimit = value;
                RaisePropertyChanged("UpperLimit");
                OnNodeModified();
            }
        }

        [JsonProperty(PropertyName = "LowerLimit")]
        public double LowerLimit
        {
            get => lowerLimit;
            set {
                lowerLimit = value;
                RaisePropertyChanged("LowerLimit");
                OnNodeModified();
            }
        }

        [JsonIgnore]
        public List<double> ListNumbers
        {
            get => listNumbers;
            set {
                listNumbers = value;
                RaisePropertyChanged("ListNumbers");
                OnNodeModified();
            }
        }

        private double mainControlWidth;
        private double mainControlHeight;

        [JsonProperty(PropertyName = "MainControlHeight")]
        public double MainControlHeight
        {
            get { return mainControlHeight; }
            set {
                mainControlHeight = value;
                RaisePropertyChanged("MainControlHeight");
                OnNodeModified();
            }
        }

        [JsonProperty(PropertyName = "MainControlWidth")]
        public double MainControlWidth
        {
            get { return mainControlWidth; }
            set {
                mainControlWidth = value;
                RaisePropertyChanged("MainControlWidth");
                OnNodeModified();
            }
        }

        private double fixed1X;
        private double fixed1Y;
        private double fixed2X;
        private double fixed2Y;

        public double Fixed1X
        {
            get { return fixed1X; }
            set { fixed1X = value; }
        }

        public double Fixed1Y
        {
            get { return fixed1Y; }
            set { fixed1Y = value; }
        }

        public double Fixed2X
        {
            get { return fixed2X; }
            set { fixed2X = value; }
        }
        public double Fixed2Y
        {
            get { return fixed2Y; }
            set { fixed2Y = value; }
        }

        private double free1X;
        private double free1Y;
        private double free2X;
        private double free2Y;

        public double Free1X
        {
            get { return free1X; }
            set { free1X = value; }
        }

        public double Free1Y
        {
            get { return free1Y; }
            set { free1Y = value; }
        }

        public double Free2X
        {
            get { return free2X; }
            set { free2X = value; }
        }

        public double Free2Y
        {
            get { return free2Y; }
            set { free2Y = value; }
        }

        private GraphTypes graphType;
        public GraphTypes GraphType
        {
            get => graphType;
            set
            {
                graphType = value;

                RaisePropertyChanged("GraphType");

                OnNodeModified();
            }
        }


        //private int selectedGraphType;
        //private Dictionary<string, int> dicGraphTypes;

        //[JsonIgnore]
        //public Dictionary<string, int> DicGraphTypes
        //{
        //    get { return dicGraphTypes; }
        //    set { dicGraphTypes = value;
        //        RaisePropertyChanged("DicGraphTypes");
        //    }
        //}

        //[JsonProperty(PropertyName = "SelectedGraphType")]
        //public int SelectedGraphType
        //{
        //    get { return selectedGraphType; }
        //    set { selectedGraphType = value;
        //        RaisePropertyChanged("SelectedGraphType");
        //    }
        //}

        //  Canvas Size
        [JsonIgnore]
        public double CanvasWidth = 0.0;
        [JsonIgnore]
        public double CanvasHeight = 0.0;

        //  Linear Points
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointFree PointLinear1 { get; set; }
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointFree PointLinear2 { get; set; }
        [JsonIgnore]
        public curvelinear CurveLinear { get; set; }
        //

        //  Bezier Curve
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointFree PointBezierControl1 { get; set; }
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointFree PointBezierControl2 { get; set; }
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointOrtho PointBezierFix1 { get; set; }
        [JsonConverter(typeof(StringToPointThumbConverter))]
        public CxControlPointOrtho PointBezierFix2 { get; set; }
        [JsonIgnore]
        public controlline CurveBezierControlLine1 { get; set; }
        [JsonIgnore]
        public controlline CurveBezierControlLine2 { get; set; }
        [JsonIgnore]
        public curvebezier CurveBezier { get; set; }
        //

        public GraphMapNodeModel()
        {
            RegisterAllPorts();

            PortDisconnected += GraphMapNodeModel_PortDisconnected;

            ArgumentLacing = LacingStrategy.Disabled;

            MainControlWidth = 300;
            MainControlHeight = 350;

            //DicGraphTypes = new Dictionary<string, int>();
            //DicGraphTypes.Add("Linear", 0);
            //DicGraphTypes.Add("Bezier", 1);
            //DicGraphTypes.Add("Parabolic", 2);
            //DicGraphTypes.Add("Sine Wave", 3);
            //DicGraphTypes.Add("Perlin Noise", 4);
            //DicGraphTypes.Add("Power", 5);
            //DicGraphTypes.Add("Reverse Power", 6);

            //SelectedGraphType = 0;

            GraphType = GraphTypes.Linear;

            LowerLimit = 0;
            UpperLimit = 100;
            Count = 10;

            //Fixed1X = 0;
            //Fixed1Y = 0;
            //Fixed2X = MainControlWidth;
            //Fixed2Y = MainControlHeight;
            //Free1X = 0;
            //Free1Y = 0;
            //Free2X = MainControlWidth;
            //Free2Y = MainControlHeight;

            if (CanvasWidth == 0.0)
                CanvasWidth = 286.0;
            if (CanvasHeight == 0.0)
                CanvasHeight = 286.0;

            PointBezierControl1 = new CxControlPointFree(new System.Windows.Point(50, 100), CanvasWidth, CanvasHeight);
            PointBezierControl2 = new CxControlPointFree(new System.Windows.Point(CanvasWidth - 50, 100), CanvasWidth, CanvasHeight);
            PointBezierFix1 = new CxControlPointOrtho(new System.Windows.Point(0, CanvasHeight), true, CanvasWidth, CanvasHeight);
            PointBezierFix2 = new CxControlPointOrtho(new System.Windows.Point(CanvasWidth, CanvasHeight), true, CanvasWidth, CanvasHeight);

            PointLinear1 = new CxControlPointFree(new System.Windows.Point(0, CanvasHeight), CanvasWidth, CanvasHeight);
            PointLinear2 = new CxControlPointFree(new System.Windows.Point(CanvasWidth, 0), CanvasWidth, CanvasHeight);
        }

        [JsonConstructor]
        public GraphMapNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            PortDisconnected += GraphMapNodeModel_PortDisconnected;
            PropertyChanged += GraphMapNodeModel_PropertyChanged;

            //DicGraphTypes = new Dictionary<string, int>();
            //DicGraphTypes.Add("Linear", 0);
            //DicGraphTypes.Add("Bezier", 1);
            //DicGraphTypes.Add("Parabolic", 2);
            //DicGraphTypes.Add("Sine Wave", 3);
            //DicGraphTypes.Add("Perlin Noise", 4);
            //DicGraphTypes.Add("Power", 5);
            //DicGraphTypes.Add("Reverse Power", 6);

            ArgumentLacing = LacingStrategy.Disabled;

            //SelectedGraphType = 0;
        }

        private void GraphMapNodeModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void GraphMapNodeModel_PortDisconnected(PortModel obj)
        {
            if (obj.PortType == PortType.Input && this.State == ElementState.Active)
            {
                RaisePropertyChanged("DataUpdated");
            }
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
            var inputs = data as ArrayList;

            var lowerlimit = double.Parse(inputs[0].ToString());
            var upperlimit = double.Parse(inputs[1].ToString());
            var count = int.Parse(inputs[2].ToString());

            RaisePropertyChanged("DataUpdated");
        }

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].IsConnected || !InPorts[1].IsConnected || !InPorts[2].IsConnected)
            {
                return new[]
                {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()),
                };
            }

            var ogtype = AstFactory.BuildIntNode((int)GraphType);
            var ocount = inputAstNodes[2];
            var limitmin = inputAstNodes[0];
            var limitmax = inputAstNodes[1];
            var omaxx = AstFactory.BuildDoubleNode(MainControlWidth);
            var omaxy = AstFactory.BuildDoubleNode(MainControlHeight);
            var ofix1x = AstFactory.BuildDoubleNode(Fixed1X);
            var ofix1y = AstFactory.BuildDoubleNode(Fixed1Y);
            var ofix2x = AstFactory.BuildDoubleNode(Fixed2X);
            var ofix2y = AstFactory.BuildDoubleNode(Fixed2Y);
            var ofre1x = AstFactory.BuildDoubleNode(Free1X);
            var ofre1y = AstFactory.BuildDoubleNode(Free1Y);
            var ofre2x = AstFactory.BuildDoubleNode(Free2X);
            var ofre2y = AstFactory.BuildDoubleNode(Free2Y);

            var ofix1 = AstFactory.BuildExprList(new List<AssociativeNode> { ofix1x, ofix1y });
            var ofix2 = AstFactory.BuildExprList(new List<AssociativeNode> { ofix2x, ofix2y });
            var ofre1 = AstFactory.BuildExprList(new List<AssociativeNode> { ofre1x, ofre1y });
            var ofre2 = AstFactory.BuildExprList(new List<AssociativeNode> { ofre2x, ofre2y });

            var functioncall = AstFactory.BuildFunctionCall(
                new Func<int, int, double, double, double, double, List<double>, List<double>, List<double>, List<double>, List<double>>(CeleryXFunctions.GraphMapCX.GenerateValues),
                new List<AssociativeNode>
                {
                    ogtype,
                    ocount,
                    limitmin,
                    limitmax,
                    omaxx, omaxy,
                    ofix1, ofix2,
                    ofre1, ofre2
                }
                );

            //  Replace this later.
            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functioncall),
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                    VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputAstNodes)
                    )
                    ),
            };
        }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum GraphTypes
    {
        [Description("Linear Curve")]
        Linear = 0,
        [Description("Bezier Curve")]
        Bezier,
        //[Description("Sine Wave")]
        //Sine,
        //[Description("Parabolic Curve")]
        //Parabola,
        //[Description("Perlin Noise")]
        //PerlinNoise,
    }
}
