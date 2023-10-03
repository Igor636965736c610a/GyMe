using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;
using GymAppInfrastructure.Models.User;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.MyMapper;
using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.Services;

internal class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IGyMeMapper _gyMeMapper;

    public UserService(IUserRepo userRepo, IMapper mapper, IUserContextService userContextService, IGyMeMapper gyMeMapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _userContextService = userContextService;
        _gyMeMapper = gyMeMapper;
    }

    public async Task RemoveFriend(Guid userToRemoveId)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        if (userIdFromJwt == userToRemoveId)
            throw new InvalidOperationException("You can't remove yourself from friend list");
        var friend1 = await _userRepo.GetFriend(userIdFromJwt, userToRemoveId);
        var friend2 = await _userRepo.GetFriend(userToRemoveId, userIdFromJwt);
        if (friend1 is null)
            throw new InvalidOperationException("you don't have this friend");
        if (friend2 is null)
            throw new InvalidProgramException("Something went wrong");
        await _userRepo.RemoveFriend(new List<UserFriend>()
        {
            friend1,
            friend2
        });
    }

    public async Task AddFriend(Guid userToAddId)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        if (userIdFromJwt == userToAddId)
            throw new InvalidOperationException("You can't be friend with yourself");
        var userToAdd = await _userRepo.GetOnlyValid(userToAddId);
        if (userToAdd is null)
            throw new NullReferenceException("User does not exist");
        var friendStatus1 = await _userRepo.GetFriend(userIdFromJwt, userToAddId);
        var friendStatus2 = await _userRepo.GetFriend(userToAddId, userIdFromJwt);
        await SetFriendStatus(userIdFromJwt, userToAddId, friendStatus1, friendStatus2);
    }

    public async Task<IEnumerable<GetUserDto>> FindUsers(string key, int page, int size)
    {
        var users = await _userRepo.FindUsers(key, page, size);

        return users.Select(x => _gyMeMapper.GetUserDtoMap(x));
    }

    public async Task<GetUserDto> GetUser(Guid id)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(id);
        if(user is null)
            throw new NullReferenceException("User does not exist");
        
        var userDto = _gyMeMapper.GetUserDtoMap(user);

        var friendStatus = await _userRepo.GetFriend(userIdFromJwt, id);
        if (friendStatus is not null)
            userDto.FriendStatus = friendStatus.FriendStatus.ToStringFast();
        
        return userDto;
    }

    public async Task<IEnumerable<GetUserDto>> GetFriends(FriendStatusDto friendStatusDto, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;
        var friendsStatus = (FriendStatus)friendStatusDto;
        
        var friends = await _userRepo.GetFriends(userIdFromJwt, friendsStatus, page, size);

        var friendsDto = _gyMeMapper.GetUserDtoMap(friends);

        return friendsDto;
    }

    public async Task<IEnumerable<CommonFriendsResultDto>> GetCommonFriends(int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if(user is null)
            throw new InvalidProgramException("Something went wrong");

        var commonFriends = await _userRepo.GetCommonFriendsSortedByCount(userIdFromJwt, page, size);
        var commonFriendsDto = _mapper.Map<IEnumerable<CommonFriendsResult>, IEnumerable<CommonFriendsResultDto>>(commonFriends);
        
        return commonFriendsDto;
    }

    public async Task BlockUser(Guid userId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var friendStatus1 = await _userRepo.GetFriend(userIdFromJwt, userId);
        var friendStatus2 = await _userRepo.GetFriend(userId, userIdFromJwt);

        await SetBlockedStatus(userIdFromJwt, userId, friendStatus1, friendStatus2);
    }

    public async Task UnblockUser(Guid userId)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var friendStatus1 = await _userRepo.GetFriend(userIdFromJwt, userId);
        if(friendStatus1 is null)
            throw new InvalidOperationException("This user is not blocked");
        if (friendStatus1.FriendStatus != FriendStatus.Blocking)
            throw new InvalidOperationException("This user is not blocked");
        var friendStatus2 = await _userRepo.GetFriend(userId, userIdFromJwt);
        if (friendStatus2 is null)
            throw new InvalidProgramException("Something went wrong");
        await _userRepo.RemoveFriend(new[] { friendStatus1, friendStatus2 });
    }
    
    private async Task SetFriendStatus(Guid userIdFromJwt, Guid user2Id, UserFriend? friendStatus1, UserFriend? friendStatus2)
    {
        if (friendStatus1 is null)
        {
            friendStatus1 = new UserFriend()
            {
                UserId = userIdFromJwt,
                FriendId = user2Id,
                FriendStatus = FriendStatus.InviteSend
            };
            friendStatus2 = new UserFriend()
            {
                UserId = user2Id,
                FriendId = userIdFromJwt,
                FriendStatus = FriendStatus.InviteReceived
            };
            await _userRepo.AddFriend(new[] { friendStatus1, friendStatus2 });
            return;
        }

        if (friendStatus2 is null)
            throw new InvalidProgramException("Something went wrong");

        switch (friendStatus1.FriendStatus)
        {
            case FriendStatus.InviteReceived:
            {
                friendStatus1.FriendStatus = FriendStatus.Friend;
                friendStatus2.FriendStatus = FriendStatus.Friend;
                await _userRepo.UpdateFriendsStatus(new[] { friendStatus1, friendStatus2 });
                break;
            }
            case FriendStatus.Friend:
            {
                throw new InvalidOperationException("This user is already your friend");
            }
            case FriendStatus.Blocked:
            {
                throw new InvalidOperationException("This user blocked you");
            }
        }
    }

    private async Task SetBlockedStatus(Guid userIdFromJwt, Guid user2Id, UserFriend? friendStatus1, UserFriend? friendStatus2)
    {
        if (friendStatus1 is null)
        {
            friendStatus1 = new UserFriend()
            {
                UserId = userIdFromJwt,
                FriendId = user2Id,
                FriendStatus = FriendStatus.Blocking
            };
            friendStatus2 = new UserFriend()
            {
                UserId = user2Id,
                FriendId = userIdFromJwt,
                FriendStatus = FriendStatus.Blocked
            };
            await _userRepo.AddFriend(new[] { friendStatus1, friendStatus2 });
            return;
        }
        if (friendStatus2 is null)
            throw new InvalidProgramException("Something went wrong");

        friendStatus1.FriendStatus = FriendStatus.Blocking;
        friendStatus2.FriendStatus = FriendStatus.Blocked;
    }
}