using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entity
{
    public class Taster
    {
        public int TasterId { get; set; }
        [Required]
        public string TasterName { get; set; }
        public string TasterSecondName { get; set; }
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
    }
}
