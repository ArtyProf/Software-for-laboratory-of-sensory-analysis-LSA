using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entity
{
    public class Tasting
    {
        public int TastingId { get; set; }
        public string TastingName { get; set; }
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
    }
}
