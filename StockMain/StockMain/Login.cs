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
    public partial class Login : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        public Login()
        {
            InitializeComponent();
            txtUserName.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Focus();
            txtUserName.Text = "";
            txtPassword.Text="";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            string constr = "Data Source=HOME;Initial Catalog=Stok;Persist Security Info=True;User ID=sa;Password=123";
            con = new SqlConnection(constr);
            string query = "Select * from Login Where UserName='" + txtUserName.Text + "' and Password='" + txtPassword.Text + "'";
         
            da = new SqlDataAdapter(query, con);
            dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count==1)
            {
                StockMain main = new StockMain();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("User Id or Password was wrong!...","Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnClear_Click(sender, e);
            }
           
        }
    }
}
