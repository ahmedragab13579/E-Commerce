using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class ProductRecommendation
    {
        public int Id { get; set; }

       
        public int ProductId { get; set; }
        public Product Product { get; set; }

       
        public int RecommendedProductId { get; set; }
        public Product RecommendedProduct { get; set; }

        public double Score { get; set; }
    }
}
