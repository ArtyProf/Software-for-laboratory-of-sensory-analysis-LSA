using LSA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Interfaces
{
    public interface ITastingHistory
    {
        Task<IList<TastingHistory>> GetTastingHistories();
        Task CreateTastingHistory(TastingHistory tastingHistory);
    }
}
