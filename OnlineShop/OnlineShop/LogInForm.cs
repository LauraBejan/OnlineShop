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
using System.Data.Common;

namespace OnlineShop
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
            label3.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxName.Text)
                && !string.IsNullOrWhiteSpace(textBoxSurname.Text))
            {
                this.Hide();
                Form1 form1 = new Form1(textBoxName.Text, textBoxSurname.Text);
                form1.Show();
            }
            else
            {
                label3.Text = "Name/surname can't be null.";
                label3.Show();
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string surname = textBoxSurname.Text;

            Connection connection = new Connection();

            if (connection._connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            var command = new Oracle.ManagedDataAccess.Client.OracleCommand("GETUSERORDERS1", connection._connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("nume", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = name;
            command.Parameters.Add("prenume", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = surname;
            Oracle.ManagedDataAccess.Client.OracleParameter p_rc = command.Parameters.Add("rc", OracleDbType.RefCursor,
                        DBNull.Value,
                        System.Data.ParameterDirection.Output);
            List<Order> orders_list = new List<Order>();
           // Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("l_cursor", OracleDbType.RefCursor);
         //   output.Direction = System.Data.ParameterDirection.ReturnValue;
            command.ExecuteNonQuery();

            //  Oracle.ManagedDataAccess.Client.OracleDataReader reader = command.ExecuteReader();
            // Dim reader As OracleDataReader


            DbDataReader reader =
                command.ExecuteReader(CommandBehavior.SequentialAccess);
            //   var reader = command.Parameters("rc").Value;

            while ( reader.Read())
            {
                Order order = new Order(reader.GetInt32(0));
                orders_list.Add(order);
            }

            connection.Close();

            seeOrdersForm seeOrdersForm = new seeOrdersForm(connection,orders_list);
        }
    }
}
