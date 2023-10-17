using FluentValidation;
using GyMeApplication.Models.User;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class PutUserDtoValidator : AbstractValidator<PutUserDto>
{
    public PutUserDtoValidator()
    {
        RuleFor(x => x.FirstName).Length(EntitiesConfig.UserConf.FirstNameMinLenght, EntitiesConfig.UserConf.FirstNameMaxLength).NotEmpty();
        RuleFor(x => x.LastName).Length(EntitiesConfig.UserConf.LastNameMinLenght, EntitiesConfig.UserConf.LastNameLength).NotEmpty();
        RuleFor(x => x.IsChlopak).NotNull();
        RuleFor(x => x.PrivateAccount).NotNull();
        RuleFor(x => x.UserName).Length(EntitiesConfig.UserConf.UserNameMinLenght, EntitiesConfig.UserConf.UserNameMaxLength);
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.ExtendedUserConf.DescriptionMaxLenght);
    }
}