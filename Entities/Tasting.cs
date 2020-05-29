using LSA.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entities
{
    public class Tasting : IBlockChain
    {
        [Key]
        public int TastingId { get; set; }
        [Display(Name = "Tasting Name")]
        public string TastingName { get; set; }
        [Display(Name = "Status")]
        public bool IsFinished { get; set; } = false;
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
        public virtual ICollection<ProductToTasting> ProductToTastings { get; set; }
        public IList<TastingHistory> TastingHistory { get; set; }
    }
}
