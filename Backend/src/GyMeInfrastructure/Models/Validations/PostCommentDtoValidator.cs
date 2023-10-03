using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GyMeInfrastructure.Models.Validations;

public class PostCommentDtoValidator : AbstractValidator<PostCommentDto>
{
    public PostCommentDtoValidator()
    {
        RuleFor(x => x.Message).MaximumLength(EntitiesConfig.CommentConf.MessageMaxLenght).NotEmpty();
    }
}