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
        [Display(Name = "Email")]
        public string TasterEmail { get; set; }
        [Display(Name = "First Name")]
        public string TasterName { get; set; }
        [Display(Name = "Second Name")]
        public string TasterSecondName { get; set; }
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
        public virtual ICollection<TastingHistory> TastingHistory { get; set; }
    }
}
