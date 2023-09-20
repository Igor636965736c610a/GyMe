using NetEscapades.EnumGenerators;

namespace GymAppCore.Models.Entities;

[EnumExtensions]
public enum FriendStatus
{
    InviteSend,
    InviteReceived,
    Friend,
    Blocked,
    Blocking
}