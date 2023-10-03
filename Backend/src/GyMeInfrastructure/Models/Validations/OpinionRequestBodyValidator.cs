using FluentValidation;
using GymAppInfrastructure.Models.InternalManagement;

namespace GymAppInfrastructure.Models.Validations;

public class OpinionRequestBodyValidator : AbstractValidator<OpinionRequestBody>
{
    public OpinionRequestBodyValidator()
    {
        RuleFor(x => x.Message).MaximumLength(2000).NotEmpty();
    }
}