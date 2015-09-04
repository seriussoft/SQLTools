using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nTools.SqlTools;

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
            
        }
    }
}
