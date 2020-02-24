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
    public class AssetValidator : AbstractValidator<Asset>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IIdentityRepository _identityRepository;

        public AssetValidator(IAssetRepository assetRepository, IRackRepository rackRepository, IModelRepository modelRepository, IIdentityRepository identityRepository)
        {
            _assetRepository = assetRepository;
            _rackRepository = rackRepository;
            _modelRepository = modelRepository;
            _identityRepository = identityRepository;

            // default rules
            RuleFor(asset => asset.Hostname)
                .NotEmpty()
                .MaximumLength(63)
                .Matches("^[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]{0,61}[a-zA-Z0-9]$")
                .MustAsync(HaveUniqueHostname);
            RuleFor(asset => asset.Rack)
                .NotNull()
                .MustAsync(RackExists)
                .When(asset => asset.Rack != null);
            RuleFor(asset => asset.Model)
                .NotNull()
                .MustAsync(ModelExists)
                .When(asset => asset.Model != null);
            RuleFor(asset => asset.RackPosition)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            RuleFor(asset => asset.Owner)
                .MustAsync(UserExists)
                .When(asset => asset.Owner != null);
            RuleFor(asset => asset)
                .Must(NotConflictWithInstalledAssets)
                .WithErrorCode("Conflict")
                .WithMessage(asset => $"Asset (hostname: {asset.Hostname}) conflicts with other installed assets.");
            RuleFor(asset => asset)
                .Must(FitWithinRack)
                .WithErrorCode("DoesNotFit")
                .WithMessage(asset =>
                    $"Asset (hostname: {asset.Hostname}) does not fit within confines of its rack.");
        }

        private static bool NotConflictWithInstalledAssets(Asset asset)
        {
            return !asset.Rack.Assets
                .Where(o => o != asset)
                .Any(asset.ConflictsWith);
        }

        private static bool FitWithinRack(Asset asset)
        {
            return !asset.SlotsOccupied()
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

        private async Task<bool> HaveUniqueHostname(Asset asset, string hostname, CancellationToken cancellationToken)
        {
            return await _assetRepository.AssetIsUniqueAsync(asset.Hostname, asset.Id);
        }

        private async Task<bool> RackExists(Rack rack, CancellationToken cancellationToken)
        {
            return await _rackRepository.AddressExistsAsync(rack.Row, rack.Column, rack.Datacenter.Id);
        }

    }
}
