namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IPresentationService
    {
        IEnumerable<SelectListItem> GetSelectItemsFromEnum<T>(int indexOfSelectedItem = 0)
            where T : struct, Enum;
    }
}
