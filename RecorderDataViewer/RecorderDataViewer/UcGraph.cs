using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace RecorderDataViewer
{
    public partial class UcGraph : UserControl
    {

        //ZedGraph.Chart chart = new ZedGraph.Chart();
        ZedGraphControl zedChart = new ZedGraphControl();

        GraphPane myPane;
        PointPairList listCH1 = new PointPairList();
        PointPairList listCH2 = new PointPairList();
        PointPairList listCH3 = new PointPairList();
        PointPairList listCH4 = new PointPairList();
        PointPairList listCH5 = new PointPairList();
        PointPairList listCH6 = new PointPairList();
        PointPairList listCH7 = new PointPairList();
        PointPairList listCH8 = new PointPairList();
        PointPairList listCH9 = new PointPairList();
        PointPairList listCH10 = new PointPairList();
        PointPairList listAlarm1 = new PointPairList();
        PointPairList listAlarmOut = new PointPairList();
        public UcGraph()
        {
            InitializeComponent();
            InitChart();
        }
        private void InitChart()
        {
            InitZed(zedChart);
            zedChart.Dock = DockStyle.Fill;
            //panel.Controls.Add(chart);
            CreateGraph(zedChart);
            panel.Controls.Add(zedChart);
            myPane = zedChart.GraphPane;
        }
        // Build the Chart
        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Tester";
            //x-axis
            myPane.XAxis.Title.Text = "Time";
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.Scale.Format = "yyyy-MM-dd HH:mm:ss";
            myPane.XAxis.Scale.MajorStepAuto = true;
            //myPane.XAxis.

            myPane.YAxis.Title.Text = "Voltage";

            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            //PointPairList list1 = new PointPairList();
            //PointPairList list2 = new PointPairList();
            //for (int i = 0; i < 36; i++)
            //{
            //    x = (double)i + 5;
            //    y1 = 1.5 + Math.Sin((double)i * 0.2);
            //    y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
            //    list1.Add(x, y1);
            //    list2.Add(x, y2);
            //}

            //// Generate a red curve with diamond
            //// symbols, and "Porsche" in the legend
            //LineItem myCurve = myPane.AddCurve("Porsche",
            //      list1, Color.Red, SymbolType.Diamond);

            //// Generate a blue curve with circle
            //// symbols, and "Piper" in the legend
            //LineItem myCurve2 = myPane.AddCurve("Piper",
            //      list2, Color.Blue, SymbolType.Circle);

            //LineItem myCurve2 = myPane.AddCurve("Piper",  list2, Color.Blue, SymbolType.Circle);
            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }
        private void InitZed(ZedGraphControl zg1)
        {
            zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            zg1.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            zg1.IsAutoScrollRange = true;
            zg1.IsEnableHEdit = false;
            zg1.IsEnableHPan = true;
            zg1.IsEnableHZoom = true;
            zg1.IsEnableVEdit = false;
            zg1.IsEnableVPan = true;
            zg1.IsEnableVZoom = true;
            zg1.IsPrintFillPage = true;
            zg1.IsPrintKeepAspectRatio = true;
            zg1.IsScrollY2 = false;
            zg1.IsShowContextMenu = true;
            zg1.IsShowCopyMessage = true;
            zg1.IsShowCursorValues = true;
            zg1.IsShowHScrollBar = true;
            zg1.IsShowPointValues = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsSynchronizeXAxes = false;
            zg1.IsSynchronizeYAxes = false;
            zg1.IsZoomOnMouseCenter = false;
            zg1.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            zg1.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            zg1.Location = new System.Drawing.Point(12, 12);
            zg1.Name = "zg1";
            zg1.PanButtons = System.Windows.Forms.MouseButtons.Left;
            zg1.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            zg1.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            zg1.PointDateFormat = "g";
            zg1.PointValueFormat = "G";
            zg1.ScrollMaxX = 0;
            zg1.ScrollMaxY = 0;
            zg1.ScrollMaxY2 = 0;
            zg1.ScrollMinX = 0;
            zg1.ScrollMinY = 0;
            zg1.ScrollMinY2 = 0;
            zg1.Size = new System.Drawing.Size(439, 297);
            zg1.TabIndex = 0;
            zg1.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            zg1.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            zg1.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            zg1.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            zg1.ZoomStepFraction = 0.1;

        }
        public void InitData()
        {
            GraphPane myPane = zedChart.GraphPane;
            myPane.CurveList.Clear();
            List<PointPairList> pointList = new List<PointPairList>();
            for (int i = 0; i < 12; i++)
            {
                pointList.Add(new PointPairList());
            }
            listCH1 = new PointPairList();
            listCH2 = new PointPairList();
            listCH3 = new PointPairList();
            listCH4 = new PointPairList();
            listCH5 = new PointPairList();
            listCH6 = new PointPairList();
            listCH7 = new PointPairList();
            listCH8 = new PointPairList();
            listCH9 = new PointPairList();
            listCH10 = new PointPairList();
            listAlarm1 = new PointPairList();
            listAlarmOut = new PointPairList();
        }
        //public void AddData(Dictionary<DateTime,Data> dicData,List<DateTime> listHigh,List<DateTime> listLow)
        public void AddData(Dictionary<DateTime, Data> dicData, List<Data> listHigh, List<Data> listLow)
        {
            try
            {
                InitData();
                foreach (var item in dicData)
                {
                    double x = (double)new XDate(item.Key);
                    listCH1.Add(x, item.Value.CH1);
                    listCH2.Add(x, item.Value.CH2);
                    listCH3.Add(x, item.Value.CH3);
                    listCH4.Add(x, item.Value.CH4);
                    listCH5.Add(x, item.Value.CH5);
                    listCH6.Add(x, item.Value.CH6);
                    listCH7.Add(x, item.Value.CH7);
                    listCH8.Add(x, item.Value.CH8);
                    listCH9.Add(x, item.Value.CH9);
                    listCH10.Add(x, item.Value.CH10);
                    listAlarm1.Add(x, item.Value.Alarm1);
                    listAlarmOut.Add(x, item.Value.AlarmOut);
                }
                myPane.CurveList.Clear();
                LineItem myCurve1 = myPane.AddCurve("CH1 ", listCH1, Properties.Settings.Default.LineColorCH1, SymbolType.None);
                LineItem myCurve2 = myPane.AddCurve("CH2 ", listCH2, Properties.Settings.Default.LineColorCH2, SymbolType.None);
                LineItem myCurve3 = myPane.AddCurve("CH3 ", listCH3, Properties.Settings.Default.LineColorCH3, SymbolType.None);
                LineItem myCurve4 = myPane.AddCurve("CH4 ", listCH4, Properties.Settings.Default.LineColorCH4, SymbolType.None);
                LineItem myCurve5 = myPane.AddCurve("CH5 ", listCH5, Properties.Settings.Default.LineColorCH5, SymbolType.None);
                LineItem myCurve6 = myPane.AddCurve("CH6 ", listCH6, Properties.Settings.Default.LineColorCH6, SymbolType.None);
                LineItem myCurve7 = myPane.AddCurve("CH7 ", listCH7, Properties.Settings.Default.LineColorCH7, SymbolType.None);
                LineItem myCurve8 = myPane.AddCurve("CH8 ", listCH8, Properties.Settings.Default.LineColorCH8, SymbolType.None);
                LineItem myCurve9 = myPane.AddCurve("CH9 ", listCH9, Properties.Settings.Default.LineColorCH9, SymbolType.None);
                LineItem myCurve10 = myPane.AddCurve("CH10", listCH10, Properties.Settings.Default.LineColorCH10, SymbolType.None);
                LineItem myCurve11 = myPane.AddCurve("Alarm1", listAlarm1, Properties.Settings.Default.LineColorCH11, SymbolType.None);
                LineItem myCurve12 = myPane.AddCurve("AlarmOut", listAlarmOut, Properties.Settings.Default.LineColorCH12, SymbolType.None);

                zedChart.AxisChange();
                this.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public void Clear()
        {
            GraphPane myPane = zedChart.GraphPane;
            myPane.CurveList.Clear();
            this.Refresh();
        }
    }
}
