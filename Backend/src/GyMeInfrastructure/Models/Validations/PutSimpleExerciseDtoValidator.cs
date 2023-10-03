using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.SimpleExercise;

namespace GyMeInfrastructure.Models.Validations;

public class PutSimpleExerciseDtoValidator : AbstractValidator<PutSimpleExerciseDto>
{
    public PutSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}