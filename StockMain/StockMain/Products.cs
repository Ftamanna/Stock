using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockMain
{
    public partial class Products : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        string constr = "Data Source=HOME;Initial Catalog=Stok;Persist Security Info=True;User ID=sa;Password=123";
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            BindGrid();
        }

        public void BindGrid()
        {
            con = new SqlConnection(constr);
            //Reading Data
            da = new SqlDataAdapter("Select * from Products", con);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "De-Active";
                }
            }
           
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {           
            bool status = false;
            if(comboBox1.SelectedIndex==0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            con = new SqlConnection(constr);
            var sqlQuery = "";
            if(IfProductExist(con,textBox1.Text))
            {
                sqlQuery= @"UPDATE [dbo].[Products] SET [ProductCode] = '"+textBox1.Text+"',[ProductName] = '"+textBox2.Text+"',[ProductStatus] = '"+status+"' WHERE[ProductCode] = '"+textBox1.Text+"'";
            }
            else
            {
                sqlQuery = @"INSERT INTO [dbo].[Products]([ProductCode],[ProductName] ,[ProductStatus]) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
            }
            cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            int save=cmd.ExecuteNonQuery();
            con.Close();

            if (save > 1)
            {
                MessageBox.Show("Your Transaction is Succed!", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
                BindGrid();
            }
            else
            {
                MessageBox.Show("Your Transaction is Failed!...", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);

            }

        }
        private bool IfProductExist(SqlConnection con,string productcode)
        {
            da = new SqlDataAdapter("Select 1 from Products where ProductCode='"+productcode+"'", con);
            dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count>=1)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.Text = "Active";
            }
            else
            {
                comboBox1.Text = "In-Active";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            var sqlQuery = "";
            if (IfProductExist(con, textBox1.Text))
            {
                sqlQuery = @"delete from Products where ProductCode='"+textBox1.Text+"'";
                cmd = new SqlCommand(sqlQuery, con);
                con.Open();
                int delete = cmd.ExecuteNonQuery();
                if(delete>1)
                {
                    MessageBox.Show("Record Deleted Successfully!...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                    BindGrid();
                }
                con.Close();
            }
            else
            {
                MessageBox.Show("Record Not Exist!...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);          
            }
         
        }
    }
}
