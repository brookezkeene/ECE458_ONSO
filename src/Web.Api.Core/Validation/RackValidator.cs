using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Validation
{
    public sealed class RackValidator : AbstractValidator<Rack>
    {
        private readonly IRackRepository _rackRepository;

        public RackValidator(IRackRepository rackRepository)
        {
            _rackRepository = rackRepository;

            RuleSet("delete", () =>
            {
                RuleFor(rack => rack.Assets)
                    .Empty();
            });
            RuleSet("create", () =>
            {
                RuleFor(rack => rack)
                    .MustAsync(HaveUniqueAddress)
                    .WithMessage(rack => $"A rack already exists at address {rack.Row}{rack.Column}.")
                    .WithSeverity(Severity.Info);
            });
        }

        private async Task<bool> HaveUniqueAddress(Rack rack, CancellationToken cancellationToken)
        {
            var exists = await _rackRepository.AddressExistsAsync(rack.Row, rack.Column);
            return !exists;
        }
    }


}
