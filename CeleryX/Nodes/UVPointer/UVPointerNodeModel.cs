using CeleryX.CommonControls;
using CeleryX.CommonControls.Subcontrols;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CeleryX.Nodes.UVPointer
{
    [NodeName("UVPointer")]
    [NodeCategory("CeleryX.UI")]
    [NodeDescription("This node contains a point that is slidable inside a rectangular area. The rectangular area normally have limits 0,0 and 1,1.")]
    [OutPortDescriptions("The result of the bounded slider calculated from the slider position.")]
    [OutPortNames("u", "v")]
    [OutPortTypes("double", "double")]
    [IsDesignScriptCompatible]
    public class UVPointerNodeModel : NodeModel
    {
        private double _u;
        private double _v;

        [JsonProperty(PropertyName = "U")]
        public double U
        {
            get => _u;
            set
            {
                _u = value;
                RaisePropertyChanged("U");
                OnNodeModified();
            }
        }

        [JsonProperty(PropertyName = "V")]
        public double V {
            get => _v;
            set
            {
                _v = value;
                RaisePropertyChanged("V");
                OnNodeModified();
            }
        }

        private double _mainControlWidth;
        private double _mainControlHeight;

        [JsonProperty(PropertyName = "MainControlHeight")]
        public double MainControlHeight
        {
            get { return _mainControlHeight; }
            set { _mainControlHeight = value;
                RaisePropertyChanged("MainControlHeight");
            }
        }

        [JsonProperty(PropertyName = "MainControlWidth")]
        public double MainControlWidth
        {
            get { return _mainControlWidth; }
            set { _mainControlWidth = value;
                RaisePropertyChanged("MainControlWidth");
            }
        }


        [JsonIgnore]
        public double CanvasWidth = 350;
        [JsonIgnore]
        public double CanvasHeight = 350;

        [JsonIgnore]
        public CxControlPointFree pointMover { get; set; }

        [JsonIgnore]
        public Crosshair CrossHairHorizontal { get; set; }
        [JsonIgnore]
        public Crosshair CrossHairVertical { get; set; }
        [JsonIgnore]
        public Uvcoordtext UVCoordinateText { get; set; }
        [JsonIgnore]
        public List<Gridline> GridLines { get; set; }

        [JsonConstructor]
        public UVPointerNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            this.PortDisconnected += UVPointerNodeModel_PortDisconnected;

            MainControlHeight = 350.0;
            MainControlWidth = 350.0;

            pointMover = new CxControlPointFree(new System.Windows.Point(U, V), MainControlWidth, MainControlHeight);

            //
            CanvasWidth = MainControlWidth;
            CanvasHeight = MainControlHeight;
            //

            LayoutGridlines();

            ArgumentLacing = LacingStrategy.Disabled;
        }

        public UVPointerNodeModel()
        {
            RegisterAllPorts();

            MainControlHeight = 350.0;
            MainControlWidth = 350.0;

            //
            CanvasWidth = MainControlWidth;
            CanvasHeight = MainControlHeight;
            //

            this.PortDisconnected += UVPointerNodeModel_PortDisconnected;

            U = MainControlWidth * 0.5;
            V = MainControlHeight * 0.5;

            pointMover = new CxControlPointFree(new System.Windows.Point(U, V), MainControlWidth, MainControlHeight);

            LayoutGridlines();

            ArgumentLacing = LacingStrategy.Disabled;
        }

        private void LayoutGridlines()
        {
            GridLines = new List<Gridline>();
            for (double i = 0.1; i < 1.0; i += 0.1)
            {
                Gridline glx = new Gridline(i, CanvasWidth, CanvasHeight);
                glx.PathCurve.Stroke = Brushes.DarkGray;
                Gridline gly = new Gridline(i, CanvasWidth, CanvasHeight, false);
                gly.PathCurve.Stroke = Brushes.DarkGray;

                GridLines.Add(glx);
                GridLines.Add(gly);
            }
        }

        private void UVPointerNodeModel_PortDisconnected(PortModel obj)
        {
            //throw new NotImplementedException();
        }

        //protected override void OnBuilt()
        //{
        //    base.OnBuilt();
        //    VMDataBridge.DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
        //}

        //public override void Dispose()
        //{
        //    base.Dispose();
        //    VMDataBridge.DataBridge.Instance.UnregisterCallback(GUID.ToString());
        //}

        //private void DataBridgeCallback(object data)
        //{
        //    ArrayList arraylist = data as ArrayList;
        //    if (arraylist.Count == 1)
        //    {

        //    }
        //}

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            double uu = U / CanvasWidth;
            double vv = (CanvasHeight - V) / CanvasHeight;

            uu = Math.Round(uu, 3);
            vv = Math.Round(vv, 3);

            var uvalue = AstFactory.BuildDoubleNode(uu);
            var vvalue = AstFactory.BuildDoubleNode(vv);
            //var functioncall = AstFactory.BuildFunctionCall(
            //    new Func<double, double, double>(CeleryXFunctions.UVPointerCX.GetUVPointerOutputValue),
            //    new List<AssociativeNode> {  inputAstNodes[0], inputAstNodes[1] }
            //    );

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), uvalue),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), vvalue)
            };
        }
    }
}
