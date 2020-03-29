using System;

namespace Web.Api.Core.Helpers
{
    public static class RackAddressedExtensions
    {
        public static string GetRowLetter(this IRackAddressed obj)
        {
            var address = obj.RackAddress;
            GuardValidAddress(address);

            return address.Substring(0, 1);
        }

        public static int GetRackNumber(this IRackAddressed obj)
        {
            var address = obj.RackAddress;
            GuardValidAddress(address, out var rackNumber);

            return rackNumber;
        }

        private static void GuardValidAddress(string address, out int col)
        {
            if (!char.IsLetter(address, 0) || !int.TryParse(address.Substring(1), out col))
            {
                throw new ArgumentException($"Attempted to generate invalid rack address, {address}");
            }
        }

        private static void GuardValidAddress(string address)
        {
            GuardValidAddress(address, out _);
        }
    }
}