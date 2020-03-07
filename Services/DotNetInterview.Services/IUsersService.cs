namespace DotNetInterview.Services
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetById<T>(string id);

        void Updade<T>(T user);
    }
}
