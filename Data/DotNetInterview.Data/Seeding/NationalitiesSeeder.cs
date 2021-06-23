namespace DotNetInterview.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;

    public class NationalitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Nationalities.Any())
            {
                return;
            }

            foreach (var nationality in await this.GetNationalities())
            {
                await dbContext.Nationalities.AddAsync(new Nationality { CompanyNationality = nationality });
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task<IEnumerable<string>> GetNationalities()
        {
            var nalionalities = await File.ReadAllLinesAsync(GlobalConstants.NationalitiesFileName);

            return nalionalities;
        }
    }
}
