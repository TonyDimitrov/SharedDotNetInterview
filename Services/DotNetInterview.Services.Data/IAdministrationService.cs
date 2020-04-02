namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;
    using DotNetInterview.Web.ViewModels.Administration.Users;

    public interface IAdministrationService
    {
        IEnumerable<T> GetAllDeletedUsers<T>();

        T GetDetailsDeletedUser<T>(string userId);

        Task UndeleteUser(string userId);

        IEnumerable<T> GetDeletedInterviews<T>();

        T GetDetailsDeletedInterview<T>(string interviewId);

        Task UndeleteInterview(string interviewId);

        ManageNationalitiesVM GetNationalities();
    }
}
