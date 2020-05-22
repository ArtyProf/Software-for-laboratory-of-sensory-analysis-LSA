using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entities
{
    public class Taster
    {
        [Key]
        public int TasterId { get; set; }
        [Required]
        public string TasterEmail { get; set; }
        public string TasterName { get; set; }
        public string TasterSecondName { get; set; }
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
        public virtual ICollection<TastingHistory> TastingHistory { get; set; }
    }
}
