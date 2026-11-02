using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Common
{
    /// <summary>
    /// Extension methods for enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Creates a dropdown list with one item for every member of the enum.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The items, with the member name as the text and its numeric value as the value.</returns>
        public static IEnumerable<SelectListItem> GetDropdown<TEnum>() where TEnum : struct, Enum
        {
            var result = Enum.GetValues<TEnum>()
                             .Select(v => new SelectListItem(v.ToString(), Convert.ToInt32(v).ToString()))
                             .ToList();

            return result;
        }

        /// <summary>
        /// Generate a sequence of Tout items for every member of the enum TEnum by applying the provided selector to
        /// each member's name and numeric value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type whose members are enumerated.</typeparam>
        /// <typeparam name="Tout">The result type produced by the selector.</typeparam>
        /// <param name="type">An instance of TEnum that identifies the enum type to enumerate.</param>
        /// <param name="resultSelect">A function that receives an enum member's name and its numeric value (as a string) and returns a Tout
        /// instance.</param>
        /// <returns>An <![CDATA[IEnumerable<Tout>]]> containing the results of invoking resultSelect for each enum member.</returns>
        public static IEnumerable<Tout> GetEnumListItems<TEnum, Tout>(this TEnum type, Func<string, string, Tout> resultSelect) where TEnum : struct, Enum
        {
            var result = Enum.GetValues<TEnum>()
                             .Select(v =>
                             {
                                 string text = v.ToString();
                                 string value = ((int)(object)v).ToString();
                                 Tout model = resultSelect(text, value);

                                 return model;
                             })
                             .ToList();

            return result;
        }
    }
}
