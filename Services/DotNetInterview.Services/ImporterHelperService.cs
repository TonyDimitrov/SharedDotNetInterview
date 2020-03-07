using DotNetInterview.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetInterview.Services
{
    public class ImporterHelperService : IImporterHelperService
    {
        private readonly ApplicationDbContext db;

        public ImporterHelperService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public T GetAll<T>()
        {
            return (T)this.db.Nationalities
                .Select(n => n.CompanyNationality)
                .ToList()
                .OrderBy(n => n);
        }
    }
}
