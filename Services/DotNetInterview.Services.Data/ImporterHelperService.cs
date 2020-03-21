namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DotNetInterview.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ImporterHelperService : IImporterHelperService
    {
        private readonly ApplicationDbContext db;

        public ImporterHelperService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SelectListItem> GetAll()
        {
            return this.db.Nationalities
                .Select(n => new SelectListItem { Text = n.CompanyNationality })
                .ToList()
                .OrderBy(n => n.Text);
        }
    }
}
