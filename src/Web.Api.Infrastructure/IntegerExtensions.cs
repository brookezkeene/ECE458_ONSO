using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Infrastructure
{
    public static class IntegerExtensions
    {
        public static bool Between(this int i, int start, int end)
        {
            return i >= start && i <= end;
        }
    }
}
