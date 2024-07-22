using Ecom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Dingue",
                    Email = "Dingue@gmail.com",
                    UserName = "Dingue",
                    Address = new Address
                    {
                        FirstName = "Dingue",
                        LastName = "Alaa",
                        City = "Ghardaia",
                        State = "Algeria",
                        Street = "Ben Smara",
                        ZipCode = "47017",
                    }

                };
                await userManager.CreateAsync(user,"Dingue@01");


            }
        }
    }
}
