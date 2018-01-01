using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Model
{
    public class Order
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Number { get; set; }
        public int Price { get; set; }
        public int Deleted { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string PostNumber { get; set; }
    }
}
