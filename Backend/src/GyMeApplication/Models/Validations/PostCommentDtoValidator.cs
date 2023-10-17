using FluentValidation;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class PostCommentDtoValidator : AbstractValidator<PostCommentDto>
{
    public PostCommentDtoValidator()
    {
        RuleFor(x => x.Message).MaximumLength(EntitiesConfig.CommentConf.MessageMaxLenght).NotEmpty();
    }
}