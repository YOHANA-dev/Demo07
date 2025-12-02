using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.AccountViewModels;
using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser? ValidateUser(LoginViewModel LoginVM)
        {
            var User = _userManager.FindByEmailAsync(LoginVM.Email).Result;
            if (User is null) return null;

            var IsPasswordValid = _userManager.CheckPasswordAsync(User, LoginVM.Password).Result;
            return IsPasswordValid ? User : null;
        }
    }
}
