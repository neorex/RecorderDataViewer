using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecorderDataViewer
{
    public partial class SubConfig : Form
    {
        Color[] LineColors = new Color[12];
        Button[] ButtonColors = new Button[12];
        public SubConfig()
        {
            InitializeComponent();
            InitClass();
        }
        private void InitClass()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    InitClass();
                });
            }
            LineColors[0] = Properties.Settings.Default.LineColorCH1;
            LineColors[1] = Properties.Settings.Default.LineColorCH2;
            LineColors[2] = Properties.Settings.Default.LineColorCH3;
            LineColors[3] = Properties.Settings.Default.LineColorCH4;
            LineColors[4] = Properties.Settings.Default.LineColorCH5;
            LineColors[5] = Properties.Settings.Default.LineColorCH6;
            LineColors[6] = Properties.Settings.Default.LineColorCH7;
            LineColors[7] = Properties.Settings.Default.LineColorCH8;
            LineColors[8] = Properties.Settings.Default.LineColorCH9;
            LineColors[9] = Properties.Settings.Default.LineColorCH10;
            LineColors[10] = Properties.Settings.Default.LineColorCH11;
            LineColors[11] = Properties.Settings.Default.LineColorCH12;

            ButtonColors[0] = btnColorCH1;
            ButtonColors[1] = btnColorCH2;
            ButtonColors[2] = btnColorCH3;
            ButtonColors[3] = btnColorCH4;
            ButtonColors[4] = btnColorCH5;
            ButtonColors[5] = btnColorCH6;
            ButtonColors[6] = btnColorCH7;
            ButtonColors[7] = btnColorCH8;
            ButtonColors[8] = btnColorCH9;
            ButtonColors[9] = btnColorCH10;
            ButtonColors[10] = btnColorCH11;
            ButtonColors[11] = btnColorCH12;

            for (int i = 0; i < 12; i++)
            {
                ButtonColors[i].BackColor = LineColors[i];
            }

            tbRealValueMin.Text = Properties.Settings.Default.RealValueMin.ToString();
            tbResolutionMin.Text= Properties.Settings.Default.ResolutionMin.ToString();
            tbRealValueMax.Text = Properties.Settings.Default.RealValueMax.ToString();
            tbResolutionMax.Text = Properties.Settings.Default.ResolutionMax.ToString();
            tbSamplingInterval.Text = Properties.Settings.Default.SamplingInterval.ToString();
            tbHeaderSize.Text = Properties.Settings.Default.HeaderSize.ToString();
        }
        private void btnColorCH_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    btnColorCH_Click(sender, e);
                });
            }
            Button Sender = sender as Button;
            int tag = Convert.ToInt32(Sender.Tag);

            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = Sender.BackColor;
            if (colorDialog.ShowDialog()==DialogResult.OK)
            {
                LineColors[tag] = colorDialog.Color;
                Sender.BackColor = colorDialog.Color;
                switch (tag)
                {
                    case 0:
                        Properties.Settings.Default.LineColorCH1 = colorDialog.Color;
                        break;
                    case 1:
                        Properties.Settings.Default.LineColorCH2 = colorDialog.Color;
                        break;
                    case 2:
                        Properties.Settings.Default.LineColorCH3 = colorDialog.Color;
                        break;
                    case 3:
                        Properties.Settings.Default.LineColorCH4 = colorDialog.Color;
                        break;
                    case 4:
                        Properties.Settings.Default.LineColorCH5 = colorDialog.Color;
                        break;
                    case 5:
                        Properties.Settings.Default.LineColorCH6 = colorDialog.Color;
                        break;
                    case 6:
                        Properties.Settings.Default.LineColorCH7 = colorDialog.Color;
                        break;
                    case 7:
                        Properties.Settings.Default.LineColorCH8 = colorDialog.Color;
                        break;
                    case 8:
                        Properties.Settings.Default.LineColorCH9 = colorDialog.Color;
                        break;
                    case 9:
                        Properties.Settings.Default.LineColorCH10 = colorDialog.Color;
                        break;
                    case 10:
                        Properties.Settings.Default.LineColorCH11 = colorDialog.Color;
                        break;
                    case 11:
                        Properties.Settings.Default.LineColorCH12 = colorDialog.Color;
                        break;
                    default:
                        break;
                }
            }
            
        }

        private void SubConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    btnSave_Click(sender, e);
                });
            }
            //for (int i = 0; i < 12; i++)
            //{
            //    LineColors[i] = ButtonColors[i].BackColor;
            //}

            Properties.Settings.Default.RealValueMin = Convert.ToDouble(tbRealValueMin.Text);
            Properties.Settings.Default.ResolutionMin = Convert.ToInt32(tbResolutionMin.Text);
            Properties.Settings.Default.RealValueMax = Convert.ToDouble(tbRealValueMax.Text);
            Properties.Settings.Default.ResolutionMax = Convert.ToInt32(tbResolutionMax.Text);
            Properties.Settings.Default.SamplingInterval = Convert.ToInt32(tbSamplingInterval.Text);
            Properties.Settings.Default.HeaderSize = Convert.ToInt32(tbHeaderSize.Text);
            Properties.Settings.Default.Save();
        }
    }
}
