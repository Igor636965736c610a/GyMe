using FluentValidation;
using GyMeInfrastructure.Models.InternalManagement;

namespace GyMeInfrastructure.Models.Validations;

public class OpinionRequestBodyValidator : AbstractValidator<OpinionRequestBody>
{
    public OpinionRequestBodyValidator()
    {
        RuleFor(x => x.Message).MaximumLength(2000).NotEmpty();
    }
}