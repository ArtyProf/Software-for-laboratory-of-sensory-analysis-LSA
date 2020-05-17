using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Helpers
{
    public static class BlockHelper
    {
        public static string ConcatData(int tastingId, int viewProse, int viewColour, int bouquetClean, int bouquetIntensity, int bouquetQuality, int tasteColour, int tasteQuality, int tasteIntensity, int tasteAftertaste, int tastePotencial, int garmony, int penalty, DateTimeOffset transactionDate, string previousBlockHash)
        {
            var formatedTastingId = tastingId.ToString("F");
            var formatedViewProse = viewProse.ToString("F");
            var formatedViewColour = viewColour.ToString("F");
            var formatedBouquetClean = bouquetClean.ToString("F");
            var formatedBouquetIntensity = bouquetIntensity.ToString("F");
            var formatedTasteColour = tasteColour.ToString("F");
            var formatedBouquetQuality = bouquetQuality.ToString("F");
            var formatedTasteQuality = tasteQuality.ToString("F");
            var formatedTasteIntensity = tasteIntensity.ToString("F");
            var formatedTasteAftertaste = tasteAftertaste.ToString("F");
            var formatedTastePotencial = tastePotencial.ToString("F");
            var formatedGarmony = garmony.ToString("F");
            var formatedPenalty = penalty.ToString("F");
            var formattedDate = transactionDate.ToString("yyyy-MM-dd");

            return $"{formatedTastingId}{viewProse}{formatedViewProse}{formatedViewColour}{formatedBouquetClean}{formatedBouquetIntensity}{formatedBouquetQuality}{formatedTasteColour}{formatedTasteQuality}{formatedTasteAftertaste}{formatedTasteIntensity}{formatedTastePotencial}{formatedGarmony}{formatedPenalty}{formattedDate}{previousBlockHash}";
        }
    }
}