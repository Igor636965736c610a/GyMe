using FluentValidation;
using GymAppCore.Models.Entities.Configurations;
using GymAppInfrastructure.Models.SimpleExercise;

namespace GymAppInfrastructure.Models.Validations;

public class PostSimpleExerciseDtoValidator : AbstractValidator<PostSimpleExerciseDto>
{
    public PostSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}