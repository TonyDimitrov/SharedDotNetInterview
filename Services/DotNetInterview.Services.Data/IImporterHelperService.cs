namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IImporterHelperService
    {
        IEnumerable<SelectListItem> GetAll();
    }
}
