using System;

namespace Web.Api.Core.Dtos
{
    public abstract class PowerPortDto
    {
        protected PowerPortDto(Guid id)
        {
            Id = id;
        }

        protected PowerPortDto() { }

        public Guid Id { get; set; }
        public int Number { get; set; }
    }
}