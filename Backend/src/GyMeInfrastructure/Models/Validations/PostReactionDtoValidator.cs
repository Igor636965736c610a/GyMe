using FluentValidation;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest.BodyRequest;

namespace GyMeInfrastructure.Models.Validations;

public class PostReactionDtoValidator : AbstractValidator<PostReactionDto>
{
    public PostReactionDtoValidator()
    {
        RuleFor(x => x.SimpleExerciseId).NotNull();
        RuleFor(x => x.ReactionType).NotNull();
    }
}