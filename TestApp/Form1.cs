using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nTools.SqlTools;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MsSqlClass sql = new MsSqlClass();
            string select = "", from = "", where = "";

            try
            {
                sql.setConn("sql01", "itdedbusr", "santac1aus3", "itdedev");
                //MessageBox.Show(sql.ConnString);

                select = "SELECT " + colNamesBox.Text + " ";
                from = "FROM " + tabNamesBox.Text + " ";

                if (select.Length.Equals(0) || from.Length.Equals(0))
                {
                    throw new Exception("you need to have a select and from clause...");
                }

                if (whereClauseBox.Text.Length > 0)
                {
                    where = "(" + whereClauseBox.Text + ")";
                }


                if (!sql.query(select + from + where))
                {
                    MessageBox.Show("not logged in");
                }
                else
                {
                    //dgview1.DataSource = sql.getDataSet();
                    //dgview1.Refresh();
                    
                    switch (comboBox1.Items[comboBox1.SelectedIndex].ToString())
                    {
                        case "Table Schema":
                            dgview1.DataSource = sql.getSchema();
                            break;
                        case "Result Set":
                            dgview1.DataSource = sql.getDataTable();//sql.getDataSet().Tables[0];
                            break;
                    }
                     
                    dgview1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }

                //close connection
                sql.disconn();
            }
            catch( Exception ea)
            {
                if(sql.isConnected())
                {
                    sql.disconn();
                }

                try
                {
                    MessageBox.Show(ea.Message);
                    MessageBox.Show(ea.InnerException.Message);
                    //MessageBox.Show(ea.InnerException.InnerException.Message);
                }
                catch
                {
                    MessageBox.Show("way too deep man...");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}