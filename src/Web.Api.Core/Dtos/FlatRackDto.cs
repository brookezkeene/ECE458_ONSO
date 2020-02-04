using System;

namespace Web.Api.Core.Dtos
{
    public class FlatRackDto
    {
        public Guid Id { get; set; }
        public string RowLetter { get; set; }
        public int RackNumber { get; set; }
        public string Address => $"{RowLetter}{RackNumber}";
    }
}