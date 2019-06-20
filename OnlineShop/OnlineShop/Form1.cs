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
    public partial class Form1 : Form
    {
        int user_exists = 0;
        int max_top = 20;
        int min_top = 0;
        public Form1(String name, String surname)
        {
            InitializeComponent();
            greetingLabel.Text = "Hello, " + name + " " + surname;
            button3.Hide();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection();
            Update_DataGrid(connection);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Update_DataGrid(Connection connection)
        { 

        connection.Open();
        Available_products availableProducts = new Available_products();
        List<Available_products> availableProductsList=availableProducts.GetProducts(connection);

            // listView1.Items.Clear();
            //check if item exists already in the cart
            listView1.Clear();

            listView1.Columns.Add("Name");
            listView1.Columns.Add("Price");
            listView1.Columns.Add("Category");
            listView1.Columns.Add("Material");
            listView1.Columns.Add("Quantity");
            int count = 0;
            foreach (var product in availableProductsList)
            {
                if (count >= min_top && count <= max_top)
                {
                    count++;

                    ListViewItem item = new ListViewItem(product.product_name);
                    item.SubItems.Add(product.price.ToString());
                    item.SubItems.Add(product.category);
                    item.SubItems.Add(product.material);
                    item.SubItems.Add(product.stock.ToString());
                    item.Tag = product.available_product_id;
                    listView1.Items.Add(item);
                    listView1.Refresh();
                }
                else if (count < min_top)
                    count++;
                else break;

            }


        }

        public void update_listView2(List<Products_Info> pr_list,Connection connection)
        {
            
            List<Products_Info> products_Info_list = new List<Products_Info>();
            //  Products_Info
            // Products_Info products_Info = new Products_Info(name,Int32.Parse(price),1,category,material);

            //listView2.Clear();
            //check if item exists already in the cart
            listView2.Clear();

            listView2.Columns.Add("Name");
            listView2.Columns.Add("Category");
            listView2.Columns.Add("Quantity");
            listView2.Columns.Add("Material");
            listView2.Columns.Add("Price");
            foreach (var products_Info in pr_list)
            {
      
                //display new item in cart
                ListViewItem item = new ListViewItem(products_Info.name);
                item.SubItems.Add(products_Info.category);
                item.SubItems.Add((products_Info.quantity).ToString());
                item.SubItems.Add(products_Info.material);
                item.SubItems.Add(products_Info.price.ToString());
                listView2.Items.Add(item);
                listView2.Refresh();

            }
            
        }

        //add item to cart
        private void testBtn_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection();
            // Insert product in db list
            Products_List products_List = new Products_List();
            int max_index = products_List.GetMaxListId(connection) + 1;
            int product_id = 1;
            var selectedtItem = listView1.SelectedItems[0].Tag;
            var sel = selectedtItem.ToString();
            product_id = Int32.Parse(sel);

            List<Products_Info> products_Infos_list;

            if (user_exists != 0)
            {
                products_List.InsertProductInList(user_exists, product_id, 1, connection);
                products_Infos_list = products_List.GetDbProductsList(connection, user_exists);


            }
            else
            //insert product in db list
            {
                products_List.InsertProductInList(max_index, product_id, 1, connection);
                user_exists = max_index;
                products_Infos_list = products_List.GetDbProductsList(connection, max_index);

            }
            AdjustStock(-1, product_id, connection);

            Update_DataGrid(connection);
            //get price

            PriceLabel.Text =( GetPrice(connection, user_exists)).ToString();

            //update cart view
            update_listView2(products_Infos_list,connection);
        }
    
    public void AdjustStock(int quantity, int product_d,Connection connection)
        {
            if (connection._connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            var command = new Oracle.ManagedDataAccess.Client.OracleCommand("adjust_stock", connection._connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("list_id", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = quantity;
            command.Parameters.Add("id_product", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = product_d;
            command.ExecuteNonQuery();
            connection.Close();
        }
    public int GetPrice(Connection connection, int list_id)
        {
            int total_price = 0;
            if (connection._connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            var command = new Oracle.ManagedDataAccess.Client.OracleCommand("get_total_price1", connection._connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("list_id", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = list_id;
           // command.Parameters.Add("prenume", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = surname;
           // List<Order> orders_list = new List<Order>();
           // Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("l_cursor", OracleDbType.RefCursor);
           // output.Direction = System.Data.ParameterDirection.ReturnValue;
            Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("total_price", OracleDbType.Int32);
            command.ExecuteNonQuery();
            total_price = Int32.Parse(command.Parameters["total_price"].Value.ToString());
            connection.Close();

            return total_price;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void previousOrdersBtn_Click(object sender, EventArgs e)
        {
            string[] words = greetingLabel.Text.Split(' ');

            string name = words[1];
            string surname = words[2];

            Connection connection = new Connection();

            if (connection._connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            var command = new Oracle.ManagedDataAccess.Client.OracleCommand("GETUSERORDERS1", connection._connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("nume", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = name;
            command.Parameters.Add("prenume", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = surname;
            List<Order> orders_list = new List<Order>();
            Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = System.Data.ParameterDirection.ReturnValue;
            command.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Client.OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Order order = new Order(reader.GetInt32(0));
                orders_list.Add(order);
            }

            connection.Close();

            seeOrdersForm seeOrdersForm = new seeOrdersForm(connection,orders_list);
             

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sendBtn_Click(object sender, EventArgs e)
        {

            AddressForm addressForm = new AddressForm(user_exists);
            addressForm.Show();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection();
            min_top = max_top;
            max_top += max_top;
            if (min_top > 0)
                button3.Show();
            Update_DataGrid(connection);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            max_top = min_top;
            min_top = max_top - min_top;
            if (min_top == 0)
                button3.Hide();
            Connection connection = new Connection();

            Update_DataGrid(connection);

        }

        private void PriceLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

