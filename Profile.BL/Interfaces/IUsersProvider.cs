using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IUsersProvider : IDisposable
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserById(int id);
        List<User> GetNewUsers();
        void DeleteNewUsers(int[] userIds);
        void SetRoles(int[] userIds, List<RoleType> roles, int? specializationId = null);
        Task<ClaimsIdentity> AuthenticateAsync(string login, string password);
        Task SendRecoverMailToUser(int userId, string callbackUrl);
        Task<IdentityResult> ResetUserPasswordByToken(string token, string newPassword);
        Avatar GetAvatarByUserIdOrDefault(int userId);
        void RemoveAvatarByUserId(int userId);
        Avatar SaveUserAvatar(int userId, System.IO.Stream avatarStream, string contentType);
        string GetDefaultAvatarUrl();
    }
}
