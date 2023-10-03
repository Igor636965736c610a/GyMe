using FluentValidation;
using GymAppInfrastructure.Models.InternalManagement;

namespace GymAppInfrastructure.Models.Validations;

public class PaymentRequestModelValidator : AbstractValidator<PaymentRequestModel>
{
    public PaymentRequestModelValidator()
    {
        RuleFor(x => x.Amount).NotNull();
        RuleFor(x => x.Currency).NotEmpty();
    }
}