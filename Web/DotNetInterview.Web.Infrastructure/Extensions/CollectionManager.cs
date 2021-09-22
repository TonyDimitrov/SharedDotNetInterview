namespace DotNetInterview.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class CollectionManager
    {
        public static IEnumerable<SelectListItem> InsertCommonElementInList(this IEnumerable<SelectListItem> items, string element)
        {
            var isSelected = false;
            var listItems = items.ToList();

            if (listItems.Any(e => e.Selected))
            {
                isSelected = true;
            }

            listItems.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = element,
                Selected = !isSelected,
            });

            return listItems;
        }
    }
}
