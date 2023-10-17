using NetEscapades.EnumGenerators;

namespace GyMeApplication.Models.User;

[EnumExtensions]
public enum FriendStatusDto
{
    InviteSend,
    InviteReceived,
    Friend,
    Blocked,
    Blocking
}