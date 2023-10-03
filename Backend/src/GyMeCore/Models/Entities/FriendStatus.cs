using NetEscapades.EnumGenerators;

namespace GyMeCore.Models.Entities;

[EnumExtensions]
public enum FriendStatus
{
    InviteSend,
    InviteReceived,
    Friend,
    Blocked,
    Blocking
}