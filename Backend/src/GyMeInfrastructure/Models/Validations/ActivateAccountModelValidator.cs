using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.Account;

namespace GyMeInfrastructure.Models.Validations;

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