using FluentValidation;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeCore.Models.Entities.Configurations;

namespace GyMeApplication.Models.Validations;

public class PutCommentDtoValidator : AbstractValidator<PutCommentDto>
{
    public PutCommentDtoValidator()
    {
        RuleFor(x => x.Message).MaximumLength(EntitiesConfig.CommentConf.MessageMaxLenght).NotEmpty();
    }
}