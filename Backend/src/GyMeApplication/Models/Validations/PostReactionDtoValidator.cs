using FluentValidation;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest.BodyRequest;

namespace GyMeApplication.Models.Validations;

public class PostReactionDtoValidator : AbstractValidator<PostReactionDto>
{
    public PostReactionDtoValidator()
    {
        RuleFor(x => x.SimpleExerciseId).NotNull();
        RuleFor(x => x.ReactionType).NotNull();
    }
}