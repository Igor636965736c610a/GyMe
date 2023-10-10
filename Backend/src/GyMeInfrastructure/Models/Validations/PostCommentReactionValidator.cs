using FluentValidation;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GyMeInfrastructure.Models.Validations;

public class PostCommentReactionValidator : AbstractValidator<PostCommentReactionDto>
{
    public PostCommentReactionValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.CommentReactionType).NotNull();
    }
}