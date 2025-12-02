using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return false;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole { Name = "SuperAdmin"},
                        new IdentityRole { Name = "Admin"}
                    };

                    foreach (var role in Roles)
                    {
                        if(!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser
                    {
                        FirstName = "Marawan",
                        LastName = "Ali",
                        UserName = "MarawanAli",
                        Email = "marawanali190@gmail.com",
                        PhoneNumber = "01222883581"
                    };
                    userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();


                    var Admin = new ApplicationUser
                    {
                        FirstName = "Mohamed",
                        LastName = "Ahmed",
                        UserName = "MohamedAhmed",
                        Email = "mohamedahmed@gmail.com",
                        PhoneNumber = "01111111111"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Seeding Failed: " + ex.Message);
                return false;
            }
        }
    }
}
