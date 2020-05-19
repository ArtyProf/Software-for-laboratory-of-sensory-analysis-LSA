using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public virtual ICollection<ProductToTasting> ProductToTastings { get; set; }
        public ICollection<TastingHistory> TastingHistory { get; set; }
    }
}
