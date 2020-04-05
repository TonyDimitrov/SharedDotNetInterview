namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;
    using DotNetInterview.Web.ViewModels.Administration.Users;

    public interface IAdministrationService
    {
        IEnumerable<T> GetAllDeletedUsers<T>();

        DeletedUsersVM GetDeletedUsersByPage(int page, DeletedUsersVM usersVM, IEnumerable<DeletedUserVM> users);

        T GetDetailsDeletedUser<T>(string userId);

        Task UndeleteUser(string userId);

        IEnumerable<T> GetDeletedInterviews<T>();

        DeletedInterviewsVM GetDeletedInterviewsByPage(int page, DeletedInterviewsVM interviewsVM, IEnumerable<DeletedInterviewVM> interviews);

        T GetDetailsDeletedInterview<T>(string interviewId);

        Task UndeleteInterview(string interviewId);

        ManageNationalitiesVM GetNationalities();
    }
}
