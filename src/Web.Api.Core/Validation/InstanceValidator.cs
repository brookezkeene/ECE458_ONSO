using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Entities.Extensions;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Validation
{
    public class InstanceValidator : AbstractValidator<Instance>
    {
        private readonly IInstanceRepository _instanceRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IIdentityRepository _identityRepository;

        public InstanceValidator(IInstanceRepository instanceRepository, IRackRepository rackRepository, IModelRepository modelRepository, IIdentityRepository identityRepository)
        {
            _instanceRepository = instanceRepository;
            _rackRepository = rackRepository;
            _modelRepository = modelRepository;
            _identityRepository = identityRepository;

            // default rules
            RuleFor(instance => instance.Hostname)
                .NotEmpty()
                .MaximumLength(63)
                .Matches("^[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]{0,61}[a-zA-Z0-9]$")
                .MustAsync(HaveUniqueHostname);
            RuleFor(instance => instance.Rack)
                .NotNull()
                .MustAsync(RackExists)
                .When(instance => instance.Rack != null);
            RuleFor(instance => instance.Model)
                .NotNull()
                .MustAsync(ModelExists)
                .When(instance => instance.Model != null);
            RuleFor(instance => instance.RackPosition)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            RuleFor(instance => instance.Owner)
                .MustAsync(UserExists)
                .When(instance => instance.Owner != null);
            RuleFor(instance => instance)
                .Must(NotConflictWithInstalledInstances)
                .WithErrorCode("Conflict")
                .WithMessage(instance => $"Instance (hostname: {instance.Hostname}) conflicts with other installed instances.");
            RuleFor(instance => instance)
                .Must(FitWithinRack)
                .WithErrorCode("DoesNotFit")
                .WithMessage(instance =>
                    $"Instance (hostname: {instance.Hostname}) does not fit within confines of its rack.");
        }

        private static bool NotConflictWithInstalledInstances(Instance instance)
        {
            return !instance.Rack.Instances
                .Where(o => o != instance)
                .Any(instance.ConflictsWith);
        }

        private static bool FitWithinRack(Instance instance)
        {
            return !instance.SlotsOccupied()
                .Any(u => u > 42);
        }

        private async Task<bool> UserExists(User arg1, CancellationToken arg2)
        {
            return await _identityRepository.FindByNameAsync(arg1.UserName) != null;
        }

        private async Task<bool> ModelExists(Model arg1, CancellationToken arg2)
        {
            return await _modelRepository.ModelExistsAsync(arg1.Vendor, arg1.ModelNumber, arg1.Id);
        }

        private async Task<bool> HaveUniqueHostname(Instance instance, string hostname, CancellationToken cancellationToken)
        {
            return await _instanceRepository.InstanceIsUniqueAsync(instance.Hostname, instance.Id);
        }

        private async Task<bool> RackExists(Rack rack, CancellationToken cancellationToken)
        {
            return await _rackRepository.AddressExistsAsync(rack.Row, rack.Column);
        }
    }
}
