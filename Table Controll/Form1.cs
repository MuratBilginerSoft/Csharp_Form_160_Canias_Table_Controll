using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Table_Controll
{
    public partial class Form1 : Form
    {
        #region Methods

        public int TEST6022(string TabloAdı)
        {
            int returnval = 0;

            SqlConnection conn = new SqlConnection("Data Source=SIRIUS; Initial Catalog=TEST6022; User Id=IASUSERS; password=Aa123456;");

            string Query = "SELECT COUNT(*) FROM " + TabloAdı;

            conn.Open();

            SqlCommand command = new SqlCommand(Query, conn);
            
            command.CommandTimeout = 0;


            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                returnval = reader.GetInt32(0);
            }
            reader.Close();
            conn.Close();

            return returnval;
        }

        public int TEST6044(string TabloAdı)
        {
            int returnval = 0;

            SqlConnection conn = new SqlConnection("Data Source=SIRIUS; Initial Catalog=TEST6044; User Id=IASUSERS; password=Aa123456;");

            string Query = "SELECT COUNT(*) FROM " + TabloAdı;

            conn.Open();

            SqlCommand command = new SqlCommand(Query, conn);
            
            command.CommandTimeout = 0;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                returnval = reader.GetInt32(0);
            }
            reader.Close();
            conn.Close();

            return returnval;

        }

        public void DROPINDEX(string TabloAdı, string IndexAdı)
        {
            SqlConnection conn = new SqlConnection("Data Source=SIRIUS; Initial Catalog=TEST6044; User Id=IASUSERS; password=Aa123456;");

            Query1 = "DROP INDEX " + TabloAdı + "." + IndexAdı;

            conn.Open();

            SqlCommand command = new SqlCommand(Query1, conn);

            SqlDataReader reader = command.ExecuteReader();

            conn.Close();
        }

        public void INSERTDATA(string Script,string TabloAdı)
        {
            SqlConnection conn = new SqlConnection("Data Source=SIRIUS; Initial Catalog=TEST6044; User Id=IASUSERS; password=Aa123456;");

            conn.Open();

            string Query2 = "Delete From " + TabloAdı+"\n"+Script;

            SqlCommand command = new SqlCommand(Query2, conn);
            
            command.CommandTimeout = 0;

            SqlDataReader reader = command.ExecuteReader();

            conn.Close();
        }

        #endregion

        #region Parameters

        string Query1;

        int j1 = 0, j2 = 0, j3 = 0, j4 = 0;

        #endregion

        #region Definitions

        string[] Table = new string[0]; // Başarısız Tablolar
        string[] Table2 = new string[0]; // Başarılı Tablolar
        string[] Table3 = new string[0]; // Hatalı Tablolar
        string[] Indexed = new string[0]; // Başarısız Tablolar Indexed
        string[] Indexed2 = new string[0]; // Başarılı Tablolar Indexed
        string[] Indexed3 = new string[0]; // Hatalı Tablolar Indexed


        #endregion


        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex!=2)
            {
                if (textBox1.Text != "")
                {
                    backgroundWorker1.RunWorkerAsync();
                    checkBox1.Enabled = true;
                    checkBox2.Enabled = true;
                }

                else
                {
                    MessageBox.Show("Hiç Bir Veri Girişi Sağlanmadı");
                }
            }

            else
            {
                backgroundWorker1.RunWorkerAsync();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            j1 = 0;
            j2 = 0;
            j3 = 0;
            j4 = 0;

            if (tabControl1.SelectedIndex == 0)
            {
                label16.Text = "Durum İşleniyor...";

                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                richTextBox4.Clear();
                richTextBox5.Clear();

                string[] dizi;
                dizi = textBox1.Text.Split('\n');

                textBox1.Clear();

                for (int i = 0; i < dizi.Length; i++)
                {

                    try
                    {
                        textBox1.Text += (++j1) + " - " + dizi[i] + "\n";

                        if (dizi[i].ToString() != "" || dizi[i].ToString() != " ")
                        {
                            string s1 = TEST6022(dizi[i].ToString()).ToString();
                            string s2 = TEST6044(dizi[i].ToString()).ToString();

                            richTextBox1.AppendText(/* j1 + " - " + */ s1 + Environment.NewLine);
                            richTextBox2.AppendText(/* j1 + " - " + */ s2 + Environment.NewLine);

                            if (s1 != s2)
                            {

                                richTextBox1.Select(richTextBox1.Text.Length - s1.Length - 1, s1.Length);
                                richTextBox1.SelectionColor = Color.Red;

                                richTextBox2.Select(richTextBox2.Text.Length - s2.Length - 1, s2.Length);
                                richTextBox2.SelectionColor = Color.Red;

                                j2++;
                                Array.Resize(ref Table, j2);
                                Array.Resize(ref Indexed, j2);
                                Table[j2 - 1] = dizi[i].ToString();
                                Indexed[j2 - 1] = j1.ToString();

                                richTextBox3.Text += dizi[i].ToString() + "\n";
                            }

                            else
                            {
                                j3++;
                                Array.Resize(ref Table2, j3);
                                Array.Resize(ref Indexed2, j3);
                                Table2[j3 - 1] = dizi[i].ToString();
                                Indexed2[j3 - 1] = j1.ToString();

                                richTextBox4.Text += dizi[i].ToString() + "\n";
                            }
                        }

                    }
                    catch (Exception)
                    {
                        richTextBox1.Text += j1 + " - " + "HATA" + "\n";
                        richTextBox2.Text += j1 + " - " + "HATA" + "\n";

                        j4++;
                        Array.Resize(ref Table3, j4);
                        Array.Resize(ref Indexed3, j4);
                        Table3[j4 - 1] = dizi[i].ToString();
                        Indexed3[j4 - 1] = j1.ToString();

                        richTextBox5.Text += dizi[i].ToString() + "\n";
                    }

                    double j6 = ((double)(i+1) / (double)dizi.Length)*100;
                    int j7 = Convert.ToInt32(j6);
                    label16.Text = "Durum İşleniyor...%" + j7;
                    backgroundWorker1.ReportProgress(j7);
                }

                label9.Text = Table.Length.ToString();
                label12.Text = Table2.Length.ToString();
                label16.Text = "İşlem Tamamlandı";
            }

            else if(tabControl1.SelectedIndex == 1)
            {
                string[] dizi1;
                dizi1 = textBox2.Text.Split('\n');

                string[] dizi2;
                dizi2 = textBox3.Text.Split('\n');


                for (int i = 0; i < dizi1.Length; i++)
                {
                    try
                    {
                        DROPINDEX(dizi1[i].ToString(), dizi2[i].ToString());
                        textBox4.Text += Query1 + "\n";
                        listBox1.Items.Add("DROPED");

                    }
                    catch (Exception)
                    {
                        textBox4.Text += Query1 + "\n";
                        listBox1.Items.Add("NOT DROPED");
                    }
                }
            }

            else if (tabControl1.SelectedIndex == 2)
            {
                string[] dizi2;
                dizi2 = textBox5.Text.Split('\n');

                label16.Text = "Durum İşleniyor...";

                for (int i = 0; i < dizi2.Length; i++)
                {
                    richTextBox6.Text += "INSERT INTO TEST6044.DBO." + dizi2[i].Trim() + " SELECT * FROM TEST6022.DBO." + dizi2[i].Trim()+"\n";

                    double j6 = ((double)(i + 1) / (double)dizi2.Length) * 100;
                    int j7 = Convert.ToInt32(j6);
                    backgroundWorker1.ReportProgress(j7);

                    label16.Text = "Durum İşleniyor...%"+j7;
                }

                label16.Text = "İşlem Tamamlandı";
            }

            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox5.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] dizi3;
            dizi3 = richTextBox6.Text.Split('\n');

            string[] dizi4;
            dizi4 = textBox5.Text.Split('\n');

          
            richTextBox7.Clear();

            for (int i = 0; i < dizi3.Length - 1; i++)
            {
                try
                {
                    INSERTDATA(dizi3[i].Trim(), dizi4[i].Trim());
                    richTextBox7.Text += "DONE" + "\n";
                }

                catch (Exception ex)
                {
                    richTextBox7.Text += ex.Message + "\n";
                }

                double j6 = ((double)(i + 2) / (double)dizi3.Length) * 100;
                int j7 = Convert.ToInt32(j6);
                label16.Text = "Durum İşleniyor...%" + j7;
                backgroundWorker2.ReportProgress(j7);

            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;

            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();

            if (checkBox2.Checked==true)
            {
                for (int i = 0; i < Table.Length; i++)
                {
                    richTextBox3.Text += Table[i].ToString().Trim() + ";1" + "\n"; 

                }

                for (int i = 0; i < Table2.Length; i++)
                {
                    richTextBox4.Text += Table2[i].ToString().Trim() + ";1" + "\n";
                }

                for (int i = 0; i < Table3.Length; i++)
                {
                    richTextBox5.Text += Table3[i].ToString().Trim() + ";1" + "\n";
                }
            }

            else
            {
                for (int i = 0; i < Table.Length; i++)
                {
                    richTextBox3.Text += Table[i].ToString() + "\n";
                }

                for (int i = 0; i < Table2.Length; i++)
                {
                    richTextBox4.Text += Table2[i].ToString().Trim()+ "\n";
                }

                for (int i = 0; i < Table3.Length; i++)
                {
                    richTextBox5.Text += Table3[i].ToString().Trim() + "\n";
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;

            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();

            if (checkBox1.Checked==true)
            {
                if (Indexed.Length>0)
                {
                    for (int i = 0; i < Table.Length; i++)
                    {
                        richTextBox3.Text += Indexed[i] + " - " + Table[i].ToString() + "\n";
                    }
                }

                if (Indexed2.Length>0)
                {
                    for (int i = 0; i < Table2.Length; i++)
                    {
                        richTextBox4.Text += Indexed2[i] + " - " + Table2[i].ToString() + "\n";
                    }
                }

                if (Indexed3.Length>0)
                {
                    for (int i = 0; i < Table3.Length; i++)
                    {
                        richTextBox5.Text += Indexed3[i] + " - " + Table3[i].ToString() + "\n";
                    }
                }
            }

            else
            {
                if (Indexed.Length > 0)
                {
                    for (int i = 0; i < Table.Length; i++)
                    {
                        richTextBox3.Text += Table[i].ToString() + "\n";
                    }
                }

                if (Indexed2.Length > 0)
                {
                    for (int i = 0; i < Table2.Length; i++)
                    {
                        richTextBox4.Text += Table2[i].ToString() + "\n";
                    }
                }

                if (Indexed3.Length > 0)
                {

                    for (int i = 0; i < Table3.Length; i++)
                    {
                        richTextBox5.Text += Table3[i].ToString() + "\n";
                    }
                }
              

                

            }
        }
    }
}
