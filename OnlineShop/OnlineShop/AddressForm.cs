using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineShop.Models;


using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace OnlineShop
{
    public partial class AddressForm : Form
    {
        public int list_idd;
        public AddressForm(int list_id)
        {
            InitializeComponent();
            label6.Hide();
            list_idd = list_id;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string address = textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text;

            Connection connection = new Connection();

            if (connection._connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            var command = new Oracle.ManagedDataAccess.Client.OracleCommand("add_address", connection._connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("adresa", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = address;
            command.Parameters.Add("id_list", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = list_idd;
            command.ExecuteNonQuery();

            connection.Close();
            label6.Text = "Order registered";
            label6.Show();
        }
    }
}
