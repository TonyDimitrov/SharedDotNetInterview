namespace DotNetInterview.Services.Data.Tests
{
    using System;
    using System.Reflection;

    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels;

    public class AutoMapperMappings : IDisposable
    {
        public AutoMapperMappings()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
