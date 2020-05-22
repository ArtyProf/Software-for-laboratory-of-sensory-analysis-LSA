using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entities
{
    public class TasterToTasting
    {
        public int TasterId { get; set; }
        public Taster Taster { get; set; }
        public int TastingId { get; set; }
        public Tasting Tasting { get; set; }
    }
}
