using FluentValidation;
using GyMeApplication.Models.InternalManagement;

namespace GyMeApplication.Models.Validations;

public class OpinionRequestBodyValidator : AbstractValidator<OpinionRequestBody>
{
    public OpinionRequestBodyValidator()
    {
        RuleFor(x => x.Message).MaximumLength(2000).NotEmpty();
    }
}