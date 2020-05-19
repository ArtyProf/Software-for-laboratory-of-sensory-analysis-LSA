using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entities
{
    public class ProductToTasting
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int TastingId { get; set; }
        public Tasting Tasting { get; set; }
    }
}
