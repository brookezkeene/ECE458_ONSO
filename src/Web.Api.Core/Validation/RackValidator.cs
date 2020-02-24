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
            // default rules
            RuleFor(rack => rack.Row)
                    .NotEmpty()
                    .Matches("^[A-Z]{1}$");
            RuleFor(rack => rack.Column)
                .GreaterThanOrEqualTo(1);
            RuleSet("delete", () =>
            {
                RuleFor(rack => rack.Assets)
                    .Empty();
            });
            RuleSet("create", () =>
            {
                RuleFor(rack => rack)
                    .MustAsync(HaveUniqueAddress)
                    .WithMessage(rack => $"A rack already exists at address {rack.Row}{rack.Column} in the datacenter {rack.Datacenter.Name}.")
                    .WithSeverity(Severity.Info);

            });
        }

        private async Task<bool> HaveUniqueAddress(Rack rack, CancellationToken cancellationToken)
        {
            var exists = await _rackRepository.AddressExistsAsync(rack.Row, rack.Column, rack.Datacenter.Id);
            return !exists;
        }
    }


}
