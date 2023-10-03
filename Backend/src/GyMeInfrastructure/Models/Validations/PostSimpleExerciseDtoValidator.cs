using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.SimpleExercise;

namespace GyMeInfrastructure.Models.Validations;

public class PostSimpleExerciseDtoValidator : AbstractValidator<PostSimpleExerciseDto>
{
    public PostSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}