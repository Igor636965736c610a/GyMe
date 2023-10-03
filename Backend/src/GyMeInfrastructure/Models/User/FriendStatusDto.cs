using NetEscapades.EnumGenerators;

namespace GyMeInfrastructure.Models.User;

[EnumExtensions]
public enum FriendStatusDto
{
    InviteSend,
    InviteReceived,
    Friend,
    Blocked,
    Blocking
}