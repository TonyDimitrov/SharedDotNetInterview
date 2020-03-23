namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
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
                  .Select(n => new SelectListItem { Text = n.CompanyNationality, Value = n.CompanyNationality })
                  .ToList()
                  .OrderBy(n => n.Text);
        }

        public IEnumerable<SelectListItem> GetAllWithSelected(string selectNationality)
        {
            return this.db.Nationalities
                  .AsEnumerable()
                  .Select(n =>
                  {
                      if (selectNationality == n.CompanyNationality)
                      {
                          return new SelectListItem { Text = n.CompanyNationality, Value = n.CompanyNationality, Selected = true };
                      }
                      else
                      {
                          return new SelectListItem { Text = n.CompanyNationality, Value = n.CompanyNationality };
                      }
                  })
                  .ToList()
                  .OrderBy(n => n.Text);
        }
    }
}
