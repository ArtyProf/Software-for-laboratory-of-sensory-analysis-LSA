using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Interfaces
{
    public interface IUserAccessService
    {
        Task<bool> RegisterUserFromForm(string email, string password);
        public bool IsUserExistByEmail(string userEmail);
    }
}
