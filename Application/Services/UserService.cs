using AutoMapper;
using Final.Dto;
using Final.Entities;
using Final.Enum;
using Final.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Final.Services
{

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper, UserManager<User> userManager)
        {

            _mapper = mapper;
            _userManager = userManager;

        }
        public async Task<List<User>> GetUsers()
        {
            return await Task.FromResult(_userManager.Users.ToList());

        }
        public async Task<EUserRole> GetUserRole(string email)
        {
            var user = await GetUser(email);
            if (user != null)
            {
                return user.Role;
            }
            return EUserRole.User;
        }
        public async Task AddUser(AddUserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                throw new Exception("User already exists");
            }

            var user = _mapper.Map<User>(userDto);
            user.IsBanned = false;
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {result.Errors.FirstOrDefault().ToString()}");
            }

            await _userManager.UpdateAsync(user);
        }


        public async Task DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task UpdateUser(UpdateUserDto updateUser)
        {
            var user = _mapper.Map<User>(updateUser);
            await _userManager.UpdateAsync(user);
        }


        public async Task<User> GetUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);


        }

        public async Task<User> GetUser(string email, string password)
        {
            var user = (await _userManager.FindByEmailAsync(email));
            if (user == null)
            {
                throw new Exception("Provide valid email");
            }
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new Exception("Provide valid password");
            }
            return user;

        }

        public async Task BanUser(BanUserDto banUserDto)
        {
            var user = await _userManager.FindByEmailAsync(banUserDto.BanIssuerUserEmail.ToString());
            if (user.Role != EUserRole.Administrator)
            {
                throw new Exception("You dont have permission to ban user");
            }

            var userToBan = await _userManager.FindByEmailAsync(banUserDto.BannedUserEmail.ToString());
            if (userToBan == null)
            {
                throw new Exception("User not found");
            }
            userToBan.IsBanned = true;

            await _userManager.UpdateAsync(user);

        }


        public async Task UnBanUser(BanUserDto banUserDto)
        {
            var user = await _userManager.FindByEmailAsync(banUserDto.BanIssuerUserEmail.ToString());
            if (user.Role != EUserRole.Administrator)
            {
                throw new Exception("You dont have permission to Unban user");
            }

            var userToBan = await _userManager.FindByEmailAsync(banUserDto.BannedUserEmail.ToString());
            if (userToBan == null)
            {
                throw new Exception("User not found");
            }
            userToBan.IsBanned = false;

            await _userManager.UpdateAsync(user);

        }
    }
}
