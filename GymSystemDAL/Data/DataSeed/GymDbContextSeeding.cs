using GymSystemDAL.Data.Contexts;
using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.DataSeed
{
    public class GymDbContextSeeding
    {
        public static bool SeedData(GymSystemDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if (HasPlans && HasCategories) return false;

                if (!HasPlans)
                {
                    var Plans = LoadDataFromJsonFiles<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        dbContext.Plans.AddRange(Plans);
                    }
                }
                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFiles<Category>("categories.json");
                    if (Categories.Any())
                    {
                        dbContext.Categories.AddRange(Categories);
                    }
                }

                return dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<T> LoadDataFromJsonFiles<T>(string FileName)
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            var FilePath = Path.Combine(projectRoot, "wwwroot", "Files", FileName);

            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException();
            }

            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
