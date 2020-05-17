using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Entity
{
    public class TastingHistory
    {
        public int TastingHistoryId { get; set; }
        public Tasting Tasting { get; set; }
        public Taster Taster { get; set; }
        public int ViewProse { get; set; }
        public int ViewColour { get; set; }
        public int BouquetClean { get; set; }
        public int BouquetIntensity { get; set; }
        public int BouquetQuality { get; set; }
        public int TasteColour { get; set; }
        public int TasteIntensity { get; set; }
        public int TasteAftertaste { get; set; }
        public int TastePotencial { get; set; }
        public int TasteQuality { get; set; }
        public int Garmony { get; set; }
        public int Penalty { get; set; }
    }
}
