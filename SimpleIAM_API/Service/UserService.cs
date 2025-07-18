﻿
using SimpleIAM_API.Entity;
using SimpleIAM_API.Repository;
using SimpleIAM_API.DTO;
using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.Helper;

namespace SimpleIAM_API.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly ClinicalTrialDbContext _context;

        public UserService(IUserRepository repo, ClinicalTrialDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<UserDto> RegisterAsync(string email, string password)
        {
            var (isValid, errors) = UserRegistration.ValidateCredentials(email, password);

            if (!isValid)
            {
                throw new ArgumentException(string.Join(" ", errors));
            }

            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException("User already exists");

            var user = new User { Email = email, Password = password };
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            //return user;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email, // or user.Username depending on your model
                Groups = new List<string>() // default empty list for new users
            };
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null || user.Password != password)
                throw new InvalidOperationException("Invalid credentials");

            return user;
        }

        public async Task AssignGroupAsync(string email, int groupId)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new KeyNotFoundException("Group not found");

            bool alreadyInGroup = user.Groups.Any(g => g.GroupId == group.Id);
            if (!alreadyInGroup)
            {
                user.Groups.Add(new UserGroup
                {
                    GroupId = group.Id,
                    UserId = user.Id
                });

                await _repo.SaveChangesAsync();
            }
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                return null;

            // Something like AutoMapper is an option here but that would be overkill in this simple project
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Groups = user.Groups.Select(g => g.Group.Name).ToList()
            };
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync(); // raw User entities

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Groups = u.Groups.Select(g => g.Group.Name).ToList()
            }).ToList();
        }
    }
}
