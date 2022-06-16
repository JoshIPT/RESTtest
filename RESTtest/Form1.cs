using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace RESTtest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            if (url == "") { MessageBox.Show("You need to specify a URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                var request = new RestRequest();
                requestBox.Clear();
                requestBox.AppendText("GET " + url + Environment.NewLine);
                foreach (ListViewItem row in listView1.Items)
                {
                    var param = row.Text;
                    var val = row.SubItems[1].Text;
                    if ((param != "") || (val != "") || (param == "Unset"))
                    {
                        request.AddHeader(param, val);
                        requestBox.AppendText(Environment.NewLine + param + ": " + val);
                    }
                }
                requestBox.AppendText(Environment.NewLine);
                request.Method = Method.Get;
                doRequest(url, request);
            }
        }

        public void doRequest(string url, RestRequest headers)
        {
            var client = new RestClient(url);
            requestBox.AppendText("Sending Request...");
            Task <RestResponse> response = client.ExecuteAsync(headers);
            requestBox.AppendText(Environment.NewLine + "Received: " + response.Result.StatusDescription + Environment.NewLine);
            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                headerBox.Clear();
                foreach (var head in response.Result.Headers)
                {
                    headerBox.AppendText(head.Name + ": " + head.Value + Environment.NewLine);
                }
                bodyBox.Clear();
                try { 
                    string niceJson = JValue.Parse(response.Result.Content).ToString(Newtonsoft.Json.Formatting.Indented);
                    bodyBox.AppendText(niceJson);
                }
                catch (Exception ex)
                {
                    bodyBox.AppendText(response.Result.Content);
                }
            }
            else
            {
                MessageBox.Show("Server responded with: " + response.Result.StatusCode.ToString() + "(" + response.Result.StatusDescription + ")");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var addFrm = new addheader();
            var res = addFrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                var itm = new ListViewItem(addFrm.ParameterName);
                itm.SubItems.Add(addFrm.ParameterValue);
                listView1.Items.Add(itm);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) { listView1.SelectedItems[0].Remove(); }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var addFrm = new addheader();
            var res = addFrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                var itm = new ListViewItem(addFrm.ParameterName);
                itm.SubItems.Add(addFrm.ParameterValue);
                listView2.Items.Add(itm);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0) { listView2.SelectedItems[0].Remove(); }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            var addFrm = new addheader();
            var res = addFrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                var itm = new ListViewItem(addFrm.ParameterName);
                itm.SubItems.Add(addFrm.ParameterValue);
                listView3.Items.Add(itm);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0) { listView3.SelectedItems[0].Remove(); }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            try
            {
                var js = JValue.Parse(textBox3.Text).ToString(Newtonsoft.Json.Formatting.Indented);
                textBox3.Text = js;
                textBox3.BackColor = textBox1.BackColor;
            }
            catch (Exception ex)
            {
                textBox3.BackColor = Color.LightPink;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var url = textBox2.Text;
            if (url == "") { MessageBox.Show("You need to specify a URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                var request = new RestRequest();
                requestBox.Clear();
                requestBox.AppendText("GET " + url + Environment.NewLine);
                foreach (ListViewItem row in listView2.Items)
                {
                    var param = row.Text;
                    var val = row.SubItems[1].Text;
                    if ((param != "") || (val != "") || (param == "Unset"))
                    {
                        request.AddHeader(param, val);
                        requestBox.AppendText(Environment.NewLine + param + ": " + val);
                    }
                }
                requestBox.AppendText(Environment.NewLine);

                foreach (ListViewItem row in listView3.Items)
                {
                    var param = row.Text;
                    var val = row.SubItems[1].Text;
                    if ((param != "") || (val != "") || (param == "Unset"))
                    {
                        request.AddParameter(param, val);
                        requestBox.AppendText(Environment.NewLine + param + " => " + val);
                    }
                }
                requestBox.AppendText(Environment.NewLine);

                if (textBox3.Text != "")
                {
                    try
                    {
                        var js = JValue.Parse(textBox3.Text).ToString(Newtonsoft.Json.Formatting.None);
                        request.AddStringBody(js, DataFormat.Json);
                        request.Method = Method.Post;
                        doRequest(url, request);

                    }
                    catch (Exception ex)
                    {
                        requestBox.AppendText("JSON Validation Failure: " + Environment.NewLine + ex.Message);
                        MessageBox.Show("The provided JSON is not valid! Please correct your JSON and resubmit.", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    request.Method = Method.Post;
                    doRequest(url, request);
                }
            }
        }

        private void addToPOSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem itm = (ListViewItem)listView1.SelectedItems[0];
                listView2.Items.Add(itm);
            }
        }

        private void addToGETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem itm = (ListViewItem)listView2.SelectedItems[0];
                listView1.Items.Add(itm);
            }
        }
    }
}
