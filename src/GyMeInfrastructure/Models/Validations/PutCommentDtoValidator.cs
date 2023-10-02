using FluentValidation;
using GymAppCore.Models.Entities.Configurations;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GymAppInfrastructure.Models.Validations;

public class PutCommentDtoValidator : AbstractValidator<PutCommentDto>
{
    public PutCommentDtoValidator()
    {
        RuleFor(x => x.Message).MaximumLength(EntitiesConfig.CommentConf.MessageMaxLenght).NotEmpty();
    }
}