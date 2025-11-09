using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.DashBoard
{
    public class DashboardDto
    {
        
        public int NewUsersThisMonth { get; set; }

       
        public int PendingOrders { get; set; }

      
        public int ShippedOrders { get; set; }

     
        public int ProductsLowInStock { get; set; }

      
        public int OrdersToday { get; set; }

     
        public int OrdersThisMonth { get; set; }

    
        public int OrdersThisYear { get; set; }
    }
}
