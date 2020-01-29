namespace Web.Api.Common.Extensions
{
    public static class IntegerExtensions
    {
        public static bool Between(this int i, int start, int end)
        {
            return i >= start && i <= end;
        }
    }
}
