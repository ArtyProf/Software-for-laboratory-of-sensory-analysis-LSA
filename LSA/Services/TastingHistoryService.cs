using LSA.Data;
using LSA.Entities;
using LSA.Helpers;
using LSA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Services
{
    public class TastingHistoryService : ITastingHistory
    {
        private readonly ApplicationDbContext _context;
        public TastingHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTastingHistory(TastingHistory tastingHistory)
        {
            if (tastingHistory == null)
                throw new ArgumentNullException(nameof(tastingHistory));

            var tastingHistories = await _context.TastingHistory.ToListAsync();

            BlockChainHelper.VerifyBlockChain(tastingHistories);
            if (tastingHistories.Any(c => !c.IsValid))
            {
                throw new InvalidOperationException("Block Chain was corrupted");
            }

            string previousBlockHash = null;
            if (tastingHistories.Any())
            {
                var previousTastingHistory = tastingHistories.Last();
                tastingHistory.TastingHistoryPreviousId = previousTastingHistory.TastingHistoryId;
                previousBlockHash = previousTastingHistory.Hash;
            }

            int tastingId = await _context.Tastings.Where(c => c.IsFinished == false).Select(d => d.TastingId).FirstAsync();

            tastingHistory.TastingId = tastingId;

            tastingHistory.ProductId = 1;

            tastingHistory.TasterId = 1;

            tastingHistory.TransactionDate = DateTime.Now;

            var blockText = BlockHelper.ConcatData(tastingHistory.ViewProse,
                    tastingHistory.ViewColour, tastingHistory.BouquetClean, tastingHistory.BouquetIntensity,
                    tastingHistory.BouquetQuality, tastingHistory.TasteColour, tastingHistory.TasteQuality,
                    tastingHistory.TasteIntensity, tastingHistory.TasteAftertaste, tastingHistory.TastePotencial,
                    tastingHistory.Garmony, tastingHistory.Penalty, tastingHistory.TransactionDate, previousBlockHash);
            tastingHistory.Hash = HashHelper.Hash(blockText);

            TastingHistory itemOnSave = _context.TastingHistory.Where
                     (x => x.TastingHistoryId == tastingHistory.TastingHistoryId).FirstOrDefault();

            itemOnSave = new TastingHistory()
            {
                ViewProse = tastingHistory.ViewProse,
                ViewColour = tastingHistory.ViewColour,
                BouquetClean = tastingHistory.BouquetClean,
                BouquetIntensity = tastingHistory.BouquetIntensity,
                BouquetQuality = tastingHistory.BouquetQuality,
                TasteColour = tastingHistory.TasteColour,
                TasteQuality = tastingHistory.TasteQuality,
                TasteIntensity = tastingHistory.TasteIntensity,
                TasteAftertaste = tastingHistory.TasteAftertaste,
                TastePotencial = tastingHistory.TastePotencial,
                Garmony = tastingHistory.Garmony,
                Penalty = tastingHistory.Penalty,
                TransactionDate = tastingHistory.TransactionDate,
                Hash = tastingHistory.Hash,
                TasterId = tastingHistory.TasterId,
                TastingId = tastingHistory.TastingId,
                ProductId = tastingHistory.ProductId,
                TastingHistoryPreviousId = tastingHistory.TastingHistoryPreviousId,
            };

            _context.TastingHistory.Add(itemOnSave);

            await _context.SaveChangesAsync();
        }
    }
}
