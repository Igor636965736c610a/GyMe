using FluentValidation;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;

namespace GyMeApplication.Models.Validations;

public class PostCommentReactionValidator : AbstractValidator<PostCommentReactionDto>
{
    public PostCommentReactionValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.CommentReactionType).NotNull();
    }
}