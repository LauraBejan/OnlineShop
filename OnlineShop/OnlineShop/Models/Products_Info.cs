using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Products_Info
    {
        public int product_info_id;
        public int product_list_id;
        public int available_product_id;
        public int id_image;
        public int quantity;

        //not in db
        public string name;
        public int price;
        public string category;
        public string material;

        public Products_Info(int av_prod_id,string name,int price,int quantity,string category,string material)
        {
            this.available_product_id = av_prod_id;
            this.name = name;
            this.price = price;
            this.quantity = quantity;
            this.category = category;
            this.material = material;
        }

        public void AddProductToTheCart()
        {

        }
    }
}
