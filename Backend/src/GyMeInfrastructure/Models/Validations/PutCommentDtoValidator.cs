using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GyMeInfrastructure.Models.Validations;

public class PutCommentDtoValidator : AbstractValidator<PutCommentDto>
{
    public PutCommentDtoValidator()
    {
        RuleFor(x => x.Message).MaximumLength(EntitiesConfig.CommentConf.MessageMaxLenght).NotEmpty();
    }
}