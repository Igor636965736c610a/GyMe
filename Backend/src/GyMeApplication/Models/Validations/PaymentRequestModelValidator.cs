using FluentValidation;
using GyMeApplication.Models.InternalManagement;

namespace GyMeApplication.Models.Validations;

public class PaymentRequestModelValidator : AbstractValidator<PaymentRequestModel>
{
    public PaymentRequestModelValidator()
    {
        RuleFor(x => x.Amount).NotNull();
        RuleFor(x => x.Currency).NotEmpty();
    }
}