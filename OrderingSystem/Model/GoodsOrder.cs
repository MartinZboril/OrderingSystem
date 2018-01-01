using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Model
{
    public class GoodsOrder
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public int GoodsID { get; set; }
        public int Deleted { get; set; }
    }
}
