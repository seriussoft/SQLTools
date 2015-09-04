using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using nTools.SqlTools;
using nTools.SqlTools.Archives;

namespace SqlTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sqlBindingSource1.refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            MySqlClass sql = new MySqlClass();

            sql.setConn(sqlBindingSource1.Conn.MySqlConnStr);
            sql.query(sqlBindingSource1.Query);*/

            MySqlClass sql = new MySqlClass();
            sql.setConn("127.0.0.1", "nvanbkirk", "Vegita", "ipm");
            //sql.changeDB("ipm");

            try
            {
                sql.query("SELECT `questionID`,`sID`,`priority`,`questionText` FROM `questions`");


                StringBuilder sb = new StringBuilder();
                foreach (SqlRow row in sql.Rows())
                {
                    foreach (SqlDataClass data in row.Fields())
                    {
                        sb.AppendFormat("'{0}', ", data.getString());
                    }

                    sb.Append('\n');
                }

                sql.disconn();
                //sql.free();

                MessageBox.Show(sb.ToString());
            }
            catch (Exception eae)
            {
                MessageBox.Show(eae.Message);
            }
        }
    }
}
