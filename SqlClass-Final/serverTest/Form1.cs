using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nTools.SqlTools;
using nTools.SqlTools.Archives;

using System.Data.Sql;
using System.Data.SqlClient;

namespace serverTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MsSqlClass ms = new MsSqlClass();
            

            try
            {
                //ms.setConn(@"data source=127.0.0.1;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;", false);
                //if (!ms.setConn(@"data source=127.0.0.1;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;", false))
                if (!ms.setConn(@"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;", false))
                    foreach (string str in MsSqlClass.Errors)
                        MessageBox.Show(str);


                if (!ms.query("SELECT * FROM dbo.bob;"))
                    foreach (string str in MsSqlClass.Errors)
                        MessageBox.Show(str);

                MessageBox.Show(ms.RowCount.ToString());

                do
                {
                    MessageBox.Show(ms.getVar("string").getString());
                    listBox1.Text += string.Format("{0}\r\n", ms.getVar(1).getString());
                    listBox1.Refresh();
                }
                while (ms.next());

                ms.disconn();
            }
            catch (Exception eae)
            {
                string str = "";

                str = string.Format
                    (
                        "{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n"
                        , eae.Message
                        , eae.Source
                        , eae.StackTrace
                        , eae.TargetSite.Name
                        , eae.InnerException.Message
                        , eae.InnerException.Source
                        , eae.InnerException.StackTrace
                        , eae.InnerException.TargetSite.Name
                    );

                System.IO.StreamWriter sw = new System.IO.StreamWriter("error.rcp");
                sw.Write(str);
                sw.Close();

            }
            }
            catch (Exception ea)
            {
                MessageBox.Show(ea.Message + "\n" + ea.Source);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;";
            conn.Open();

            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = @"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;";
            conn2.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT * FROM dbo.bob", conn);
            SqlCommand cmd2 = new SqlCommand("SELECT @@ROWCOUNT", conn2);

            SqlDataReader sdr = cmd1.ExecuteReader();

            MessageBox.Show(sdr.RecordsAffected.ToString());
            sdr.Read();
            MessageBox.Show(sdr.RecordsAffected.ToString());


            //sdr.GetSchemaTable().Rows.Count

            DataGrid dg = new DataGrid();
            dg.DataSource = sdr.GetSchemaTable();
            dg.Refresh();
            Form f = new Form();
            f.Width = 400;
            f.Height = 400;
            f.Controls.Add(dg);
            dg.Dock = DockStyle.Fill;
            f.VerticalScroll.Enabled = true;
            f.HorizontalScroll.Enabled = true;
            f.ShowDialog();

            MessageBox.Show(cmd2.ExecuteScalar().ToString());
            sdr.Close();
            conn.Close();
        }

        //delegate void getRowCntDelegate(object cmd);

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;";
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.bob", conn);
            //getRowCntDelegate g = new getRowCntDelegate(getRowCnt);
            //System.Threading.ParameterizedThreadStart pts = new System.Threading.ParameterizedThreadStart(getRowCnt);
            System.Threading.Thread t = new System.Threading.Thread(this.getRowCnt);
            
            SqlDataReader sdr = cmd.ExecuteReader();

            sdr.Read();
            int y = 0;
            do
            {
                MessageBox.Show(sdr["string"].ToString());
                if (y == 1)
                {
                    t.Start(cmd.CommandText);
                }
                y++;
            }
            while(sdr.Read());

            sdr.Close();
            conn.Close();
            int cnt = 0;
            while (t.IsAlive)
            {
                cnt++;
            }
            MessageBox.Show(cnt.ToString());

        }

        public void getRowCnt(object query)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;";
            conn.Open();

            SqlCommand cmd = new SqlCommand((string)query, conn);
            cmd.ExecuteScalar();
            cmd.CommandText = "SELECT @@ROWCOUNT;";

            MessageBox.Show(cmd.ExecuteScalar().ToString());
            
            //MessageBox.Show(cmd.ExecuteScalar().ToString());

            conn.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MsSqlClass mssql = new MsSqlClass();
            mssql.setConn(@"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;");

            mssql.query("SELECT * FROM dbo.bob");

            MessageBox.Show(mssql.RowCount.ToString());

            foreach (SqlRow row in mssql.Rows())
            {
                MessageBox.Show(row["string"].getString());
            }

            mssql.disconn();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MsSqlClass mssql = new MsSqlClass();
            mssql.setConn(@"data source=192.168.1.2;initial catalog=test;integrated security=SSPI;persist security info=False;Trusted_Connection=Yes;");

            mssql.query("SELECT * FROM dbo.bobs");

            MessageBox.Show(mssql.RowCount.ToString());

            foreach (string str in MsSqlClass.Errors)
            {
                MessageBox.Show(str);
            }

            mssql.disconn();
        }

    }
}