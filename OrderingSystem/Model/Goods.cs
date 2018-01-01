using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Model
{
    public class Goods
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public int YearOfRealising { get; set; }
        public string Content { get; set; }
        public int Price { get; set; }
        public int IDcart { get; set; }
    }
}
