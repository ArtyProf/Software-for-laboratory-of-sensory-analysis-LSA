using LSA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Helpers
{
    public static class BlockChainHelper
    {
        public static void VerifyBlockChain(IList<TastingHistory> tastingHistory)
        {
            string previousHash = null;
            foreach (var entry in tastingHistory.OrderBy(c => c.TastingHistoryId))
            {
                var previousBlock = tastingHistory.SingleOrDefault(c => c.TastingHistoryId == entry.TastingHistoryPreviousId);
                var blockText = BlockHelper.ConcatData(
                    entry.ViewProse,
                    entry.ViewColour,
                    entry.BouquetClean,
                    entry.BouquetIntensity,
                    entry.BouquetQuality,
                    entry.TasteColour,
                    entry.TasteQuality,
                    entry.TasteIntensity,
                    entry.TasteAftertaste,
                    entry.TastePotencial,
                    entry.Garmony,
                    entry.Penalty,
                    entry.TransactionDate,
                    previousHash);

                var blockHash = HashHelper.Hash(blockText);

                // check current block hashes, and previous block hashes, since
                // it could have been modified in DB, ie checking the chain by two blocks at a time
                entry.IsValid = blockHash == entry.Hash && previousHash == previousBlock?.Hash;

                previousHash = blockHash;
            }
        }
    }
}
