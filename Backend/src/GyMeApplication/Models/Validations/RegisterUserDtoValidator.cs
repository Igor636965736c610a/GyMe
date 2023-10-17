using FluentValidation;
using GyMeApplication.Models.User;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.FirstName).Length(EntitiesConfig.UserConf.FirstNameMinLenght, EntitiesConfig.UserConf.FirstNameMaxLength).NotEmpty();
        RuleFor(x => x.LastName).Length(EntitiesConfig.UserConf.LastNameMinLenght, EntitiesConfig.UserConf.LastNameLength).NotEmpty();
        RuleFor(x => x.UserName).Length(EntitiesConfig.UserConf.UserNameMinLenght, EntitiesConfig.UserConf.UserNameMaxLength);
        RuleFor(x => x.GenderDto).NotNull();
        RuleFor(x => x.PrivateAccount).NotNull();
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.ExtendedUserConf.DescriptionMaxLenght);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}