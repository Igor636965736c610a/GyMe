using NetEscapades.EnumGenerators;

namespace GymAppInfrastructure.Models.User;

[EnumExtensions]
public enum FriendStatusDto
{
    InviteSend,
    InviteReceived,
    Friend,
    Blocked,
    Blocking
}