using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Order
    {
        public int list_id;
        public int max_id_user;
        public int max_id_number;
        public int max_id_order_detail;
        public int max_id_lista;
        public int max_id_product_info;
        public int max_id_image;
        public int nr_orders;
        public int nr_orders_detail;
        public int nr_product_info;

        public Order(int a)
        {
            list_id = a;
        }
    }
}
