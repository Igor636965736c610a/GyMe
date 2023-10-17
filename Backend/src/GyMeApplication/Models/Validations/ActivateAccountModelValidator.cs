using FluentValidation;
using GyMeApplication.Models.Account;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class ActivateAccountModelValidator : AbstractValidator<ActivateAccountModel>
{
    public ActivateAccountModelValidator()
    {
        RuleFor(x => x.UserName).Length(EntitiesConfig.UserConf.UserNameMinLenght, EntitiesConfig.UserConf.UserNameMaxLength).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.ExtendedUserConf.DescriptionMaxLenght);
        RuleFor(x => x.GenderDto).NotNull();
        RuleFor(x => x.PrivateAccount).NotNull();
    }
}