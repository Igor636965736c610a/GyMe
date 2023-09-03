using FluentValidation;
using GymAppCore.Models.Entities.Configurations;
using GymAppInfrastructure.Models.SimpleExercise;

namespace GymAppInfrastructure.Models.Validations;

public class PutSimpleExerciseDtoValidator : AbstractValidator<PutSimpleExerciseDto>
{
    public PutSimpleExerciseDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
    }
}