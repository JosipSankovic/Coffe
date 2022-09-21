using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Database;
namespace Ordering.Orders
{
    public struct Table
    {
        public int tableNumber;
        public int tableCost;
        public List<Pica> billItems;

        
    }

}
