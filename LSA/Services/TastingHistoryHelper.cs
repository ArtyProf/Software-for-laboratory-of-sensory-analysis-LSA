using LSA.Data;
using LSA.Entity;
using LSA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Helpers
{
    public class TastingHistoryHelper : ITastingHistory
    {
        private readonly ApplicationDbContext _context;
        public TastingHistoryHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<TastingHistory>> GetTastingHistories()
        {
            return await _context.TastingHistory.ToListAsync();
        }

        public async Task CreateTastingHistory(TastingHistory tastingHistory)
        {
            if (tastingHistory == null)
                throw new ArgumentNullException(nameof(tastingHistory));

            var tastingHistories = await _context.TastingHistory.Where(c => c.TastingHistoryId == tastingHistory.TastingHistoryId).ToListAsync();

            BlockChainHelper.VerifyBlockChain(tastingHistories);
            if (tastingHistories.Any(c => !c.IsValid))
            {
                throw new InvalidOperationException("Block Chain was invalid");
            }

            string previousBlockHash = null;
            if (tastingHistories.Any())
            {
                var previousTastingHistor = tastingHistories.Last();
                tastingHistory.TastingHistoryPreviousId = previousTastingHistor.TastingHistoryPreviousId;
                previousBlockHash = previousTastingHistor.Hash;
            }

            int tastingId = await _context.Tastings.Where(c => c.IsFinished == false).Select(d => d.TastingId).FirstOrDefaultAsync();
            tastingHistory.Tasting.TastingId = tastingId;

            tastingHistory.Product.ProductId = 1;

            tastingHistory.Taster.TasterId = 1;

            tastingHistory.TastingHistoryPreviousId = 1;

            tastingHistory.TransactionDate = DateTime.Now;
            string sample = "werwerw";
            previousBlockHash = HashHelper.Hash(sample);

            var blockText = BlockHelper.ConcatData(tastingHistory.Tasting.TastingId, tastingHistory.ViewProse,
                    tastingHistory.ViewColour, tastingHistory.BouquetClean, tastingHistory.BouquetIntensity,
                    tastingHistory.BouquetQuality, tastingHistory.TasteColour, tastingHistory.TasteQuality, 
                    tastingHistory.TasteIntensity, tastingHistory.TasteAftertaste, tastingHistory.TastePotencial,
                    tastingHistory.Garmony, tastingHistory.Penalty, tastingHistory.TransactionDate, previousBlockHash);
            tastingHistory.Hash = HashHelper.Hash(blockText);

            _context.TastingHistory.Add(tastingHistory);

            await _context.SaveChangesAsync();
        }
    }
}
