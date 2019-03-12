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
using System.IO;

namespace sec6report
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        //SqlDataAdapter da;
        //DataSet ds;
        //DataGridView r;
        public Form1()
        {
            InitializeComponent();
            string x = "bedo";
            if (x == "bedo")
                Console.WriteLine("wtf");
            else
                Console.WriteLine("fk bedo");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'prog3secDataSet.sec6reporttable' table. You can move, or remove it, as needed.
            this.sec6reporttableTableAdapter.Fill(this.prog3secDataSet.sec6reporttable);
            string s = "Data Source=ABDULRAHMAN-ABO;Initial Catalog=prog3sec;Integrated Security=true";
            con = new SqlConnection(s);
            con.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rn = Convert.ToInt32(textBox1.Text);
            string cn = textBox2.Text;
            string d = dateTimePicker1.Value.ToString();
            string pn = textBox4.Text;
            int pq = Convert.ToInt32(textBox5.Text);
            int pr = Convert.ToInt32(textBox6.Text);
            int tot = pq * pr;
            textBox7.Text = Convert.ToString(tot);
            //string[] r = { Convert.ToString(rn), cn, d, pn, Convert.ToString(pq), Convert.ToString(pr), Convert.ToString(tot) };
            //dataGridView1.Rows.Add(r);
            SqlCommand c = new SqlCommand("insert into sec6reporttable (fnum,name,date,pname,pq,pprice,tot) values('" + rn + "','" + cn + "','" + d + "','" + pn + "','" + pq + "','" + pr + "','" + tot + "')", con);
            c.ExecuteNonQuery();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            this.sec6reporttableTableAdapter.Fill(this.prog3secDataSet.sec6reporttable);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            SqlCommand c = new SqlCommand("select * from sec6reporttable where date>='" + dateTimePicker2.Value.ToString() + "' and date<='" + dateTimePicker3.Value.ToString() + "'", con);
            
            SqlDataReader d = c.ExecuteReader();
            
            while (d.Read()) {
                string rn, cn, da, pn, pq, pr, tot;
                rn = d[0].ToString();
                cn = d[1].ToString();
                da = d[2].ToString();
                pn = d[3].ToString();
                pq = d[4].ToString();
                pr = d[5].ToString();
                tot = d[6].ToString();
                string[] r = { rn, cn, da, pn, pq, pr, tot };
                dataGridView2.Rows.Add(r);                      
            }
            d.Close();
            
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.sec6reporttableTableAdapter.FillBy(this.prog3secDataSet.sec6reporttable);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            SqlCommand c = new SqlCommand("select * from sec6reporttable where fnum='" + textBox3.Text + "'", con);
            SqlDataReader d = c.ExecuteReader();
            while (d.Read())
            {
                string rn, cn, da, pn, pq, pr, tot;
                rn = d[0].ToString();
                cn = d[1].ToString();
                da = d[2].ToString();
                pn = d[3].ToString();
                pq = d[4].ToString();
                pr = d[5].ToString();
                tot = d[6].ToString();
                string[] r = { rn, cn, da, pn, pq, pr, tot };
                dataGridView2.Rows.Add(r);
            }
            d.Close();
        }
    }
}
