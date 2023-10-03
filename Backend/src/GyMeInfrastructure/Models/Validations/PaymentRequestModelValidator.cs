using FluentValidation;
using GyMeInfrastructure.Models.InternalManagement;

namespace GyMeInfrastructure.Models.Validations;

public class PaymentRequestModelValidator : AbstractValidator<PaymentRequestModel>
{
    public PaymentRequestModelValidator()
    {
        RuleFor(x => x.Amount).NotNull();
        RuleFor(x => x.Currency).NotEmpty();
    }
}