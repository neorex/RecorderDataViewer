using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecorderDataViewer
{
    public class Data
    {
        public ArrayList dataList;
        public DateTime TIME { get; set; }
        public double CH1 { get; set; }
        public double CH2 { get; set; }
        public double CH3 { get; set; }
        public double CH4 { get; set; }
        public double CH5 { get; set; }
        public double CH6 { get; set; }
        public double CH7 { get; set; }
        public double CH8 { get; set; }
        public double CH9 { get; set; }
        public double CH10 { get; set; }
        public double Alarm1 { get; set; }
        public double AlarmOut { get; set; }
        public Data(double ch1, double ch2, double ch3, double ch4, double ch5, double ch6, double ch7, double ch8, double ch9, double ch10,double alarm1,double alarmOut)
        {
            CH1 = ch1;
            CH2 = ch2;
            CH3 = ch3;
            CH4 = ch4;
            CH5 = ch5;
            CH6 = ch6;
            CH7 = ch7;
            CH8 = ch8;
            CH9 = ch9;
            CH10 = ch10;
            Alarm1 = alarm1;
            AlarmOut = alarmOut;
        }
        public Data(double[] value)
        {
            CH1 = value[0];
            CH2 = value[1];
            CH3 = value[2];
            CH4 = value[3];
            CH5 = value[4];
            CH6 = value[5];
            CH7 = value[6];
            CH8 = value[7];
            CH9 = value[8];
            CH10 = value[9];
            Alarm1 = value[10];
            AlarmOut = value[11];
        }
        public Data(DateTime time,double[] value)
        {
            TIME = time;
            CH1 = value[0];
            CH2 = value[1];
            CH3 = value[2];
            CH4 = value[3];
            CH5 = value[4];
            CH6 = value[5];
            CH7 = value[6];
            CH8 = value[7];
            CH9 = value[8];
            CH10 = value[9];
            Alarm1 = value[10];
            AlarmOut = value[11];

            AddArrayList(time, value);
        }
        private void AddArrayList(DateTime time, double[] value)
        {
            dataList = new ArrayList();
            dataList.Add(time);
            dataList.AddRange(value);
        }
    }
}
