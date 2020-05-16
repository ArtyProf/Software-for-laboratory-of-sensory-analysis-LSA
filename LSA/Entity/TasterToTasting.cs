using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entity
{
    public class TasterToTasting
    {
        public int TasterId { get; set; }
        public Taster Taster { get; set; }
        public int TastingId { get; set; }
        public Tasting Tasting { get; set; }
    }
}
