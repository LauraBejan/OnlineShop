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
    public partial class seeOrdersForm : Form
    {
        public seeOrdersForm(Connection connection,List<Order> orders_list )
        {
            InitializeComponent();
            List<Order> products_Info_list = new List<Order>();
            //  Products_Info
            // Products_Info products_Info = new Products_Info(name,Int32.Parse(price),1,category,material);

            //listView2.Clear();
            int count = 0;
            //check if item exists already in the cart
            listViewOrders.Clear();

            listViewOrders.Columns.Add("Order id");
            listViewOrders.Columns.Add("Product");

            Products_List products_List = new Products_List();

            foreach (var id_list in orders_list)
            {
                List<Products_Info> products_Infos_list = products_List.GetDbProductsList(connection, id_list.list_id);

                foreach (var products_Info in products_Infos_list)
                {

                    //display new item in cart
                    ListViewItem item = new ListViewItem(products_Info.name);
                    item.SubItems.Add(products_Info.price.ToString());
                    listViewOrders.Items.Add(item);
                    listViewOrders.Refresh();
                }

            }
        }

        private void seeOrdersForm_Load(object sender, EventArgs e)
        {

        }

        private void listViewOrders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
