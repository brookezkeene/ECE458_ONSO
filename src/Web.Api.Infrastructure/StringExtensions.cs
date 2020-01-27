using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Infrastructure
{
    public static class StringExtensions
    {
        public static bool BetweenIgnoreCase(this string str, string start, string end)
        {
            var leftComp = string.Compare(str, start, StringComparison.InvariantCultureIgnoreCase);
            var rightComp = string.Compare(str, end, StringComparison.InvariantCultureIgnoreCase);
            return leftComp >= 0 && rightComp <= 0;
        }
    }
}
