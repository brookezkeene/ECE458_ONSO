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
    public class ModelValidator : AbstractValidator<Model>
    {
        private readonly IModelRepository _modelRepository;

        public ModelValidator(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;

            // default rules
            RuleFor(model => model.Vendor)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(model => model.ModelNumber)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(model => model)
                .MustAsync(HaveUniqueVendorModelNumber)
                .WithMessage("A model already exists with that vendor and model number.");
            RuleFor(model => model.Height)
                 .InclusiveBetween(1, 42);
            RuleFor(model => model.DisplayColor)
                .Matches("^#[0-9A-Fa-f]{6}$");
            RuleFor(model => model.EthernetPorts)
                .GreaterThanOrEqualTo(0);
            RuleFor(model => model.PowerPorts)
                .GreaterThanOrEqualTo(0);
            RuleFor(model => model.Cpu)
                .MaximumLength(50);
            RuleFor(model => model.Memory)
                .GreaterThanOrEqualTo(0);
            RuleFor(model => model.Storage)
                .MaximumLength(50);

            RuleSet("update", () =>
            {
                RuleFor(model => model.Height)
                    .MustAsync(MeetHeightChangeCriteria)
                    .WithMessage("Cannot change the height of a model with existing instances.");
            });

            RuleSet("delete", () =>
            {
                // cannot have any existing instances
                RuleFor(model => model.Instances)
                    .Empty();
            });
        }

        private async Task<bool> MeetHeightChangeCriteria(Model model, int height, CancellationToken cancellationToken)
        {
            return await _modelRepository.MeetsHeightChangeCriteriaAsync(model);
        }

        private async Task<bool> HaveUniqueVendorModelNumber(Model model, CancellationToken cancellationToken)
        {
            return await _modelRepository.ModelIsUniqueAsync(model.Vendor, model.ModelNumber, model.Id);
        }
    }
}
