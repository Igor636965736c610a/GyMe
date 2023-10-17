using FluentValidation;
using GyMeApplication.Models.SimpleExercise;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class PutSimpleExerciseDtoValidator : AbstractValidator<PutSimpleExerciseDto>
{
    public PutSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}