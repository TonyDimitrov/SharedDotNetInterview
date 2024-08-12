namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PresentationService : IPresentationService
    {
        public IEnumerable<SelectListItem> GetSelectItemsFromEnum<T>(int enumIndex = 0)
            where T : struct, Enum
        {
            var result = new List<SelectListItem>();
            var values = Enum.GetValues(typeof(T));

            foreach (T enumItem in Enum.GetValues(typeof(T)))
            {
                var type = enumItem.GetType();
                var memInfo = type.GetMember(enumItem.ToString());
                var attribute = memInfo[0].GetCustomAttribute(typeof(DisplayAttribute), true);

                var getEnumString = string.Empty;
                var getEnumInt = (int)(object)enumItem;

                if (attribute is null)
                {
                    getEnumString = Enum.GetName<T>(enumItem);
                }
                else
                {
                    getEnumString = ((DisplayAttribute)attribute).Name;
                }

                var selectItem = new SelectListItem() { Value = getEnumInt.ToString(), Text = getEnumString };
                result.Add(selectItem);

                if (enumIndex == getEnumInt)
                {
                    selectItem.Selected = true;
                }
            }

            return result.OrderBy(si => si.Value);
        }
    }
}
