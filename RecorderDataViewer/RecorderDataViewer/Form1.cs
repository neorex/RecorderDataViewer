using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecorderDataViewer
{
    public enum EDGE
    {
        HIGH,
        LOW,
        NONE
    }
    public partial class frmMain : Form
    {
        SubConfig _Config = new SubConfig();
        string[] dataColumnHeader = new string[] { "Time", "CH1", "CH2", "CH3", "CH4", "CH5", "CH6", "CH7", "CH8", "CH9", "CH10", "Alarm1", "AlarmOut" };
        ExcelControl.ExcelControl _excelControl = new ExcelControl.ExcelControl();
        DataSet ds = new DataSet();
        double _threshold = 10;
        DateTime _timeStart;
        int _samplingInterval;
        int _headerSize;
        byte[] _arData;
        List<double> _listData = new List<double>();
        Dictionary<DateTime, Data> _dicData = new Dictionary<DateTime, Data>();
        List<Data> _listDatas = new List<Data>();
        List<Data> _listCH1 = new List<Data>();
        List<Data> _listCH2 = new List<Data>();
        List<Data> _listCH3 = new List<Data>();
        List<Data> _listCH4= new List<Data>();
        List<Data> _listCH5 = new List<Data>();
        List<Data> _listCH6 = new List<Data>();
        List<Data> _listCH7 = new List<Data>();
        List<Data> _listCH8 = new List<Data>();
        List<Data> _listCH9 = new List<Data>();
        List<Data> _listCH10 = new List<Data>();
        List<Data> _listCH11 = new List<Data>();
        List<Data> _listCH12 = new List<Data>();
        List<List<Data>> _listCH = new List<List<Data>>();
        //List<DateTime> _listHigh = new List<DateTime>();
        //List<DateTime> _listLow = new List<DateTime>();
        public frmMain()
        {
            InitializeComponent();
            _listCH.Add(_listCH1);
            _listCH.Add(_listCH2);
            _listCH.Add(_listCH3);
            _listCH.Add(_listCH4);
            _listCH.Add(_listCH5);
            _listCH.Add(_listCH6);
            _listCH.Add(_listCH7);
            _listCH.Add(_listCH8);
            _listCH.Add(_listCH9);
            _listCH.Add(_listCH10);
            _listCH.Add(_listCH11);
            _listCH.Add(_listCH12);

        }
        #region Parser
        private void parseData(byte[] data, ref List<double> listOutput, bool isBigEndian)
        {
            listOutput.Clear();
            byte[] buffer = new byte[2];
            int length = data.Length;
            for (int i = 0; i < length; i = i + 2)
            {
                Buffer.BlockCopy(data, i, buffer, 0, 2);
                if (!isBigEndian)
                {
                    Array.Reverse(buffer);
                }
                short output = BitConverter.ToInt16(buffer, 0);
                double volt = ConvertToVolt(output, 
                    Properties.Settings.Default.ResolutionMin, Properties.Settings.Default.ResolutionMax, Properties.Settings.Default.RealValueMin, Properties.Settings.Default.RealValueMax);
                listOutput.Add(volt);
                //Console.WriteLine(volt);
            }
        }
        private void parseContents(string fileName, bool isBigEndian)
        {
            string line;

            using (StreamReader sr = new StreamReader(fileName))
            {
                //header 분석
                while ((line = sr.ReadLine()) != null)
                {
                    string[] buffer = line.Split('=');

                    if (buffer[0].Trim() == "HeaderSize")
                    {
                        _headerSize = Convert.ToInt32(buffer[1]);
                    }
                    else if (buffer[0].Trim() == "Sample")
                    {
                        if (buffer[1].Contains("ms"))
                        {
                            string bufferinterval = buffer[1].Replace("ms", "");
                            _samplingInterval = Convert.ToInt32(bufferinterval);
                        }
                    }
                    else if (buffer[0].Trim() == "Start")
                    {
                        string[] bufferDate = buffer[1].Split(new char[] { '-', ',', ':' });
                        if (bufferDate.Length >= 6)
                        {
                            List<int> listDate = new List<int>();
                            foreach (var item in bufferDate)
                            {
                                listDate.Add(Convert.ToInt32(item));
                            }
                            try
                            {
                                _timeStart = new DateTime(listDate[0], listDate[1], listDate[2], listDate[3], listDate[4], listDate[5]);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }
                    }
                }

            }
            using (StreamReader sr = new StreamReader(fileName))
            {
                long dataSize = sr.BaseStream.Length;
                int contentsSize = (int)(dataSize - _headerSize);
                byte[] arBuffer = ReadAllBytes(sr.BaseStream);
                _arData = new byte[contentsSize];
                Buffer.BlockCopy(arBuffer, _headerSize, _arData, 0, contentsSize);

            }

        }
        private void MakeDataSet()
        {
            //_disData, _listLow, _listHigh 넘긴다
            try
            {
                ds.Tables.Clear();
                List<DataTable> listDataTable = new List<DataTable>();

                listDataTable.Add(SetDataTable<Data>(_listDatas, "MeasureData", dataColumnHeader));
                listDataTable.Add(SetDataTable<Data>(_listCH1, "EdgeHigh", dataColumnHeader));
                listDataTable.Add(SetDataTable<Data>(_listCH2, "EdgeLow", dataColumnHeader));
                //listDataTable.Add(SetDataTable<DateTime>(_listHigh, "EdgeHigh", new string[] { "Time" }));
                //listDataTable.Add(SetDataTable<DateTime>(_listLow, "EdgeLow", new string[] { "Time" }));

                ds.Tables.AddRange(listDataTable.ToArray());

                //DataTable dt0 = SetDataTable<Data>(_listDatas, "MeasureData", dataColumnHeader);
                //DataTable dt1 = SetDataTable<DateTime>(_listHigh, "EdgeHigh", new string[] { "Time" });
                //DataTable dt2 = SetDataTable<DateTime>(_listLow, "EdgeLow", new string[] { "Time" });
                
                //DataTable dtMeasureData = InitDataTable("MeasureData", dataColumnHeader);

                //foreach (var item in _listDatas)
                //{
                //    dtMeasureData.Rows.Add(item.dataList);
                //}
                //ds.Tables.Add(dtMeasureData);

                //DataTable dtEdgeHigh = InitDataTable("EdgeHigh", dataColumnHeader);

                //foreach (var item in _listDatas)
                //{
                //    dtEdgeHigh.Rows.Add(item.dataList);
                //}
                //ds.Tables.Add(dtEdgeHigh);

                //DataTable dtEdgeLow = InitDataTable("EdgeLow", dataColumnHeader);

                //foreach (var item in _listDatas)
                //{
                //    dtEdgeLow.Rows.Add(item.dataList);
                //}
                //ds.Tables.Add(dtEdgeLow);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private DataTable SetDataTable<T>(List<T> listData, string tableName, string[] columnHeader)
        {
            DataTable dt = ExportToExcel.CreateExcelFile.ListToDataTable(listData);

            //DataTable dt = new DataTable(tableName);
            //int length = columnHeader.Length;

            //try
            //{


            //    dt.Columns.Add("Date", typeof(DateTime));
            //    dt.Columns.Add("CH1", typeof(double));
            //    dt.Columns.Add("CH2", typeof(double));
            //    dt.Columns.Add("CH3", typeof(double));
            //    dt.Columns.Add("CH4", typeof(double));
            //    dt.Columns.Add("CH5", typeof(double));
            //    dt.Columns.Add("CH6", typeof(double));
            //    dt.Columns.Add("CH7", typeof(double));
            //    dt.Columns.Add("CH8", typeof(double));
            //    dt.Columns.Add("CH9", typeof(double));
            //    dt.Columns.Add("CH10", typeof(double));
            //    dt.Columns.Add("Alarm0", typeof(double));
            //    dt.Columns.Add("AlarmOff", typeof(double));

            //    for (int j = 0; j < 1000000; j++)
            //    {
            //        dt.Rows.Add(DateTime.Now, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            //}
            foreach (var item in _listDatas)
            {
                dt.Rows.Add(item.dataList.ToArray());
            }

            return dt;
        }
        private DataTable InitDataTable(string name, string[] columns)
        {
            DataTable dt = new DataTable(name);
            int length = columns.Length;
            DataColumn[] dataColumn = new DataColumn[length];
            for (int i = 0; i < length; i++)
            {
                dataColumn[i] = new DataColumn(columns[i]);
            }
            dt.Columns.AddRange(dataColumn);

            return dt;
        }

        private void MakeResult()
        {
            try
            {

                _dicData.Clear();
                int length = _listData.Count / 12;
                for (int i = 0; i < length; i++)
                {
                    DateTime time = _timeStart.AddMilliseconds(i * _samplingInterval);
                    double[] buffer = _listData.GetRange(i * 12, 12).ToArray();


                    _dicData.Add(time, new Data(buffer));

                    //listdatas
                    _listDatas.Add(new Data(time, buffer));
                    if (i > 0)
                    {
                        EDGE edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH1), Math.Abs(_dicData.Values.ElementAt(i).CH1), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH1.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH2), Math.Abs(_dicData.Values.ElementAt(i).CH2), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH2.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH3), Math.Abs(_dicData.Values.ElementAt(i).CH3), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH3.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH4), Math.Abs(_dicData.Values.ElementAt(i).CH4), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH4.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH5), Math.Abs(_dicData.Values.ElementAt(i).CH5), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH5.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH6), Math.Abs(_dicData.Values.ElementAt(i).CH6), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH6.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH7), Math.Abs(_dicData.Values.ElementAt(i).CH7), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH7.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH8), Math.Abs(_dicData.Values.ElementAt(i).CH8), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH8.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH9), Math.Abs(_dicData.Values.ElementAt(i).CH9), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH9.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).CH10), Math.Abs(_dicData.Values.ElementAt(i).CH10), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH10.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).Alarm1), Math.Abs(_dicData.Values.ElementAt(i).Alarm1), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH11.Add(new Data(time, buffer));
                        }
                        edge = CheckEdge(Math.Abs(_dicData.Values.ElementAt(i - 1).AlarmOut), Math.Abs(_dicData.Values.ElementAt(i).AlarmOut), Convert.ToDouble(upDownThreshold.Value));
                        if (edge == EDGE.HIGH)
                        {
                            _listCH12.Add(new Data(time, buffer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        #endregion
        #region Util
        public byte[] ReadAllBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        private string OpenDataFile()
        {
            string fileName = string.Empty;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                }
            }

            return fileName;
        }
        private double ConvertToVolt(short input, int resolutionMin, int resolutionMax, double outputMin, double outputMax)
        {
            double volt = 0.0;
            volt = input - resolutionMin;
            volt /= resolutionMax - resolutionMin;
            volt *= outputMax - outputMin;
            volt += outputMin;
            return volt;
        }
        private void ClearData()
        {
            _timeStart=DateTime.Now;
            _samplingInterval=Properties.Settings.Default.SamplingInterval;
            _headerSize=Properties.Settings.Default.HeaderSize;

            _listData.Clear();
            _dicData.Clear();

            _listDatas.Clear();
            //_listCH1.Clear();
            //_listCH2.Clear();
            foreach (var item in _listCH)
            {
                item.Clear();
            }
        }
        private EDGE CheckEdge(double p1, double p2, double threshold)
        {
            EDGE edge = EDGE.NONE;

            double value = (threshold - p1)*(threshold- p2);

            if (value>0)
            {
                return EDGE.NONE;
            }
            if (p1<p2)
            {
                edge = EDGE.HIGH;
            }
            else
            {
                edge = EDGE.LOW;
            }
            return edge;
        }
        #endregion
        #region Button
        private void btnConvert_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            ClearData();
            string fileName = OpenDataFile();
            if (fileName == string.Empty) return;

            parseContents(fileName, false);
            parseData(_arData, ref _listData, false);
            MakeResult();
            
            StringBuilder sb = new StringBuilder();

            DateTime from = _dicData.ElementAt(0).Key;
            DateTime to = _dicData.ElementAt(_dicData.Count - 1).Key;
            sb.AppendFormat("데이터 파일: {0}\r\n",fileName);
            sb.AppendFormat("시작 시간: {0:yyyy-MM-dd HH:mm:ss.fff}\r\n", from);
            sb.AppendFormat("종료 시간: {0:yyyy-MM-dd HH:mm:ss.fff}\r\n", to);
            sb.AppendFormat("모니터링 시간: {0}\r\n", to - from);

            //sb.AppendFormat("CH1 Edge Count : {0}\r\n", _listCH1.Count);

            //sb.AppendFormat("CH2 Edge Count : {0}\r\n", _listCH2.Count);
            int chCount = 1;
            foreach (var item in _listCH)
            {
                if (item.Count>0)
                {
                    sb.AppendFormat("CH{0} Edge Count : {1}\r\n", chCount,item.Count);
                    chCount++;
                }
            }

            textBox1.Text += sb;
            
            ucGraph.AddData(_dicData, _listCH1, _listCH2);

            this.Refresh();
            this.ResumeLayout(false);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            ucGraph.Clear();
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            
            using (SaveFileDialog sfd=new SaveFileDialog())
            {
                try
                {
                    sfd.DefaultExt = "xlsx";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = sfd.FileName;
                        MakeDataSet();

                        //new
                        _excelControl.ExportDSToExcel(ds, fileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message+"\r\n"+ex.StackTrace);
                }
            }
            
            sw.Stop();
            textBox1.Text += string.Format("Export to excel completed. {0:g}", sw.Elapsed);

        }

        #endregion

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (_Config.CanFocus)
            {
                _Config.Activate();
            }
            else
            {
                _Config.Show();
            }
        }
    }
}
                                                                                  