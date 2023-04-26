using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    public UserService(IUserRepo userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task RemoveFriend(Guid user1Id, Guid user2Id)
    {
        if (user1Id == user2Id)
            throw new InvalidOperationException("You can't remove yourself from friend list");
        var friend1 = await _userRepo.GetFriend(user1Id, user2Id);
        var friend2 = await _userRepo.GetFriend(user2Id, user1Id);
        if (friend1 is null)
            throw new InvalidOperationException("something went wrong");
        if (friend2 is null)
            throw new InvalidOperationException("you don't have this friend");
        if (!await _userRepo.RemoveFriend(new List<UserFriend>()
            {
                friend1,
                friend2
            }))
            throw new SaveChangesDbException("Something went wrong while saving database changes");
    }

    public async Task RemoveFriendRequest(Guid user1Id, Guid user2Id)
    {
        if (user1Id == user2Id)
            throw new InvalidOperationException("You can't remove yourself from friend request list");
        var friendRequest1 = await _userRepo.GetFriendRequest(user1Id, user2Id);
        if (friendRequest1 is null)
            throw new InvalidOperationException("Friend request does not exist");
        await _userRepo.RemoveFriendRequest(friendRequest1);
    }

    public async Task AddFriend(Guid user1Id, Guid user2Id)
    {
        if (user1Id == user2Id)
            throw new InvalidOperationException("You can't be friend with yourself");
        var userToAdd = await _userRepo.Get(user2Id);
        if (userToAdd is null)
            throw new NullReferenceException("User does not exist");
        var friend =  await _userRepo.GetFriend(user1Id, user2Id);
        if (friend is not null)
            throw new InvalidOperationException("This user is already your friend");
        var friendRequest = await _userRepo.GetFriendRequest(user2Id, user1Id);
        if (friendRequest is null)
        {
            if (await _userRepo.GetFriendRequest(user1Id, user2Id) is not null)
                return;
            if (!await _userRepo.AddFriendRequest(new FriendRequest{
                    SenderId = user1Id,
                    RecipientId = user2Id }))
                throw new SaveChangesDbException("Something went wrong while saving database changes");
            return;
        }
        else
        {
            await _userRepo.RemoveFriendRequest(friendRequest);
            await _userRepo.AddFriend(new List<UserFriend>()
            {
                new UserFriend()
                {
                    UserId = user1Id,
                    FriendId = user2Id
                },
                new UserFriend()
                {
                    UserId = user2Id,
                    FriendId = user1Id
                }
            });
        }
    }

    public async Task<List<GetUserDto>> FindUsers(string key, int page, int size)
    {
        var users = await _userRepo.FindUsers(key, page, size);

        var usersDto = _mapper.Map<List<User>, List<GetUserDto>>(users);

        return usersDto;
    }

    public async Task<GetUserDto> GetUser(Guid id)
    {
        var user = await _userRepo.Get(id);
        if(user is null)
            throw new NullReferenceException("User does not exist");
        
        var userDto = _mapper.Map<User, GetUserDto>(user);

        return userDto;
    }

    public async Task<IEnumerable<GetUserDto>> GetFriends(Guid userId, int page, int size)
    {
        var friends = await _userRepo.GetFriends(userId, page, size);

        var friendsDto = _mapper.Map<IEnumerable<User>, IEnumerable<GetUserDto>>(friends);

        return friendsDto;
    }
}