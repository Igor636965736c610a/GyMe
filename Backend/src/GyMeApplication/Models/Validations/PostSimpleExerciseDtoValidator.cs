using FluentValidation;
using GyMeApplication.Models.SimpleExercise;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class PostSimpleExerciseDtoValidator : AbstractValidator<PostSimpleExerciseDto>
{
    public PostSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}