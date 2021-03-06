﻿using LSA.Data;
using LSA.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSA.Services
{
    public class UserAccessService : IUserAccessService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAccessService(ApplicationDbContext context,
                                 UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> RegisterUserFromForm(string email, string password)
        {
            if (email.Length == 0 || password.Length == 0)
            {
                return false;
            }

            if (IsUserExistByEmail(email))
            {
                return false;
            }

            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var roleCheck = await _roleManager.RoleExistsAsync("Taster");
                if (!roleCheck)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Taster"));
                }

                await _userManager.AddToRoleAsync(user, "Taster");
            }

            foreach (var error in result.Errors)
            {
                return false;
            }

            return true;
        }

        public bool IsUserExistByEmail(string userEmail)
        {
            var user = _userManager.FindByEmailAsync(userEmail);
            if (user.Result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<int> GetTastingId()
        {
            return await _context.Tastings.Where(c => c.IsFinished == false).Select(d => d.TastingId).FirstAsync();
        }
    }
}
