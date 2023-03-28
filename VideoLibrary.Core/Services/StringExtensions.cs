using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VideoLibrary
{
    public static class StringExtensions
    {
        public static string GetFullErrorMessage(this ModelStateDictionary modelState)
        {
            List<string> messages = new List<string>();

            foreach (KeyValuePair<string, ModelStateEntry> entry in modelState)
                foreach (ModelError error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);

            return string.Join(" ", messages);
        }

        /// <summary>
        /// Parses a string value in to a Int32 value.
        /// </summary>
        /// <param name="Value">The string value to parse</param>
        /// <returns>The formatted Int32 value</returns>
        public static int ToInt32(this string Value)
        {
            int Result = int.Parse(Value);

            return Result;
        }
    }
}
