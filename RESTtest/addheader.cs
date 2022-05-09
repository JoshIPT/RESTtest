using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTtest
{
    public partial class addheader : Form
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        private bool canClose = false;
        public addheader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var param = textBox1.Text;
            var val = textBox2.Text;
            if (param == "") { MessageBox.Show("Please enter a parameter name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (val == "") { MessageBox.Show("Please enter a parameter value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                this.ParameterName = param;
                this.ParameterValue = val;
                this.DialogResult = DialogResult.OK;
                this.canClose = true;
                this.Close();
            }
        }

        private void addheader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
