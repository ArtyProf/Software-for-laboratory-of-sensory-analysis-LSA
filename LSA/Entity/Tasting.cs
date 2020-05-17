﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entity
{
    public class Tasting
    {
        [Key]
        public int TastingId { get; set; }
        public string TastingName { get; set; }
        public bool IsFinished { get; set; } = false;
        public virtual ICollection<TasterToTasting> TasterToTastings { get; set; }
        public ICollection<TastingHistory> TastingHistory { get; set; }
    }
}
