namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class NationalitiesService : INationalitiesService
    {
        private readonly ApplicationDbContext db;

        public NationalitiesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Nationality> GetById(int id)
        {
            return await Task.Run(() =>
            {
                return this.db.Nationalities.FirstOrDefault(n => n.Id == id);
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetAll()
        {
            return await Task.Run(() =>
            {
                return this.db.Nationalities
                       .Select(n => new SelectListItem { Text = n.CompanyNationality, Value = n.Id.ToString() })
                       .ToList()
                       .OrderBy(n => n.Text);
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetAllWithSelected(int? selectedNationalityId)
        {
            return await Task.Run(() =>
            {
                return this.db.Nationalities
                    .AsEnumerable()
                    .Select(n =>
                    {
                        if (n.Id == selectedNationalityId.Value)
                        {
                            return new SelectListItem { Text = n.CompanyNationality, Value = n.Id.ToString(), Selected = true };
                        }
                        else
                        {
                            return new SelectListItem { Text = n.CompanyNationality, Value = n.Id.ToString() };
                        }
                    })
                    .ToList()
                    .OrderBy(n => n.Text);
            });
        }

        public async Task<DbOperation> AddNationality(string nationality)
        {
            if (string.IsNullOrWhiteSpace(nationality))
            {
                return new DbOperation(false, "Error! Nationality was empty!");
            }

            if (this.db.Nationalities.Any(n => n.CompanyNationality == nationality))
            {
                return new DbOperation(false, $"Error! Nationality [{nationality}] already exists!");
            }

            await this.db.AddAsync(new Nationality { CompanyNationality = nationality });

            await this.db.SaveChangesAsync();

            return new DbOperation(true, $"Nationality [{nationality}]  successfully added!");
        }

        public async Task<DbOperation> DeleteNationality(int nationalityId)
        {
            if (nationalityId <= 0)
            {
                return new DbOperation(false, $"Error! Nationality was incorrect!");
            }

            var dbnationality = this.db.Nationalities.FirstOrDefault(n => n.Id == nationalityId);

            if (dbnationality == null)
            {
                return new DbOperation(false, $"Error! Nationality with id [{nationalityId}] was not found!");
            }

            try
            {
                this.db.Remove(dbnationality);

                await this.db.SaveChangesAsync();

                return new DbOperation(true, $"Nationality [{dbnationality.CompanyNationality}] was successfully deleted!");
            }
            catch (Exception ex)
            {
                return new DbOperation(false, $"Error! Nationality [{dbnationality.CompanyNationality}] was not deleted! It may be already related to user or interview.");
            }

        }

    }
}
