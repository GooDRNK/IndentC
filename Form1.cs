using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IndentC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string Output = string.Empty;
        string Input = string.Empty;
        int identlv = 0;
        StringBuilder final = new StringBuilder();
        string Final=string.Empty;
        string indent(int level)
        {

            return new string(' ',level);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FlushMemory.Interval = 1000;
            FlushMemory.Start();
            iTalk_TrackBar1.Value = 4;
            iTalk_Label2.Text = "Ident size: 4";
        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                Input = theDialog.FileName.ToString();
                iTalk_TextBox_Big1.Text = File.ReadAllText(Input);
            }
        }

        private void iTalk_Button_13_Click(object sender, EventArgs e)
        {  
            if (File.Exists(Input) || iTalk_TextBox_Big1.Text!=string.Empty)
            {
                var lastview = 0;
                final.Clear();
                foreach (char car in File.ReadAllText(Input))
                {
                    switch (car)
                    {
                        case '{':
                            {
                                lastview = 1;
                                final.AppendLine();
                                final.Append(indent(identlv));
                                final.Append(car);
                                final.AppendLine();
                                identlv += iTalk_TrackBar1.Value;
                                final.Append(indent(identlv));
                                break;
                            }
                        case ';':
                            {
                                final.Append(car);
                                final.AppendLine();
                                final.Append(indent(identlv));
                                break;
                            }
                        case '}':
                            {
                                lastview = 2;
                                identlv -= iTalk_TrackBar1.Value;
                                final.AppendLine();
                                final.Append(indent(identlv));
                                final.Append(car);
                                final.AppendLine();
                                final.Append(indent(identlv));
                                break;
                            }
                        case '#':
                            {
                                final.AppendLine();
                                final.Append(car);
                                break;
                            }
                        default:
                            {
                                final.Append(car);
                                break;
                            }
                    }

                }
                if(iTalk_Toggle1.Toggled)
                {
                    Final = Regex.Replace(final.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                    iTalk_TextBox_Big2.Text = Final.ToString();
                }
                else
                {
                    Final = final.ToString();
                    iTalk_TextBox_Big2.Text = Final.ToString();
                }
                Regex.Replace(Final, @"\s*([=*/+-])\s*", "$1");
                if (iTalk_Toggle2.Toggled)
                {
                    Clipboard.SetText(Final);
                }        
            }
            else
            {
                MessageBox.Show("Nu uita sa selectezi un fisier.");
            }
        }

        private void iTalk_Button_12_Click(object sender, EventArgs e)
        {
            if (iTalk_TextBox_Big2.Text != string.Empty)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FilterIndex = 2;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Output = sfd.FileName.ToString();
                        if(File.Exists(Output))
                        {
                            File.Delete(Output);
                        }
                        using (var tw = new StreamWriter(Output, true))
                        {
                            tw.WriteAsync(Final.ToString());
                            tw.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nu uita sa faci conversia sau sa selectezi un fisier.");
            }

        }

        private void iTalk_TrackBar1_ValueChanged()
        {
            iTalk_Label2.Text = "Ident size: " + iTalk_TrackBar1.Value;
        }

        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public static void FlushMemorys()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void FlushMemory_Tick(object sender, EventArgs e)
        {
            FlushMemorys();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.upit.ro/");
        }

        private void iTalk_Button_14_Click(object sender, EventArgs e)
        {
            Process.Start("https://pastebin.com/");
        }
    }
}
