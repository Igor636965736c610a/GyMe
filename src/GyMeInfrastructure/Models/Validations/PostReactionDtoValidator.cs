using FluentValidation;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest.BodyRequest;

namespace GymAppInfrastructure.Models.Validations;

public class PostReactionDtoValidator : AbstractValidator<PostReactionDto>
{
    public PostReactionDtoValidator()
    {
        RuleFor(x => x.SimpleExerciseId).NotNull();
        RuleFor(x => x.ReactionType).NotNull();
    }
}