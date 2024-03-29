﻿using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;

namespace GyMeApplication.IServices;

public interface ICommentReactionService
{
    Task<Guid> AddCommentReaction(PostCommentReactionDto postCommentReactionDto);
    Task<GetCommentReactionDto> GetCommentsReaction(Guid commentReactionId);
    Task<IEnumerable<GetCommentReactionDto>> GetCommentsReactions(Guid commentId, CommentReactionType? commentReactionType, int page, int size);
    Task<IEnumerable<GetCommentReactionCount>> GetSpecificCommentReactionCount(Guid commentId);
    Task RemoveCommentReaction(Guid commentReactionId);
}