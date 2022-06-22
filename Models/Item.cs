using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Models
{
    public class Item
    {
        //thong tin san pham
        public int Id { get; set; }
        public ItemProducts ProductRecord { get; set; }
        //so luong
        public int Quantity { get; set; }
    }
}
