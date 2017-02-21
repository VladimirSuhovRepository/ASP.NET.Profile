using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNet.Identity;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity;

namespace Profile.BL.Providers
{
    public class UsersProvider : IUsersProvider
    {
        protected readonly IProfileContext _context;

        private const string DefaultAvatarUrl = @"/images/avatar-small-placeholder.png";

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly UserManager _userManager;

        public UsersProvider(IProfileContext profileContext, UserManager userManager)
        {
            _context = profileContext;
            _userManager = userManager;
            Logger.Debug("Creating instance of provider");
        }

        public void SetRoles(int[] userIds,
                                List<RoleType> roles,
                                int? specializationId = default(int?))
        {
            var users = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();
            var specialization = specializationId.HasValue ?
                                 _context.Specializations.Find(specializationId.Value) :
                                 null;

            foreach (var user in users)
            {
                foreach (var newRole in roles)
                {
                    user.Roles.Clear();
                    CreateUserBlankByRole(user, newRole, specialization);
                    AddToRole(user, newRole);
                }
            }

            _context.SaveChanges();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Task<User> GetUserById(int id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(string login, string password)
        {
            ClaimsIdentity claims = null;
            var user = await _userManager.FindAsync(login, password);

            if (user != null)
            {
                claims = await user.GenerateUserIdentityAsync(_userManager);
            }

            return claims;
        }

        public async Task SendRecoverMailToUser(int userId, string callbackUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(userId);
                string callbackUrlWithToken = $"{callbackUrl}?token={token}";

                var siteUri = new Uri(callbackUrl); 
                string siteLink = siteUri.GetLeftPart(UriPartial.Authority);

                string mailSubject = "Восстановление пароля";
                var mailBody = new StringBuilder();

                mailBody.AppendLine($"Здравствуйте, {user.FullName}!");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine("Мы получили запрос на смену пароля для вашего аккаунта на сайте Profile.");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine($"Пожалуйста, <a href='{callbackUrlWithToken}'>пройдите по ссылке</a>, чтобы ввести новый пароль.");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine("До встречи на сайте!");
                mailBody.AppendLine("<br/>");
                mailBody.AppendLine(siteLink);

                await _userManager.SendEmailAsync(userId, mailSubject, mailBody.ToString());

                user.ResetPasswordToken = token;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<IdentityResult> ResetUserPasswordByToken(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);

            if (user == null)
            {
                return IdentityResult.Failed("Не найден пользователь с данным токеном");
            }

            var passwordVerification = _userManager.PasswordHasher.VerifyHashedPassword(
                user.PasswordHash, newPassword);

            if (passwordVerification == PasswordVerificationResult.Success)
            {
                return IdentityResult.Failed("Новый пароль должен отличаться от предыдущего");
            }

            var resetPasswordOperationResult = await _userManager.ResetPasswordAsync(user.Id, token, newPassword);

            if (resetPasswordOperationResult.Succeeded)
            {
                user.ResetPasswordToken = null;

                _context.SaveChanges();
            }

            return resetPasswordOperationResult;
        }

        public Avatar GetAvatarByUserIdOrDefault(int userId)
        {
            var userAvatar = GetAvatarByUserId(userId);

            return userAvatar ?? GetDefaultAvatar();
        }

        public void RemoveAvatarByUserId(int userId)
        {
            var userAvatar = GetAvatarByUserId(userId);

            if (userAvatar != null)
            {
                var entry = _context.Entry(userAvatar);
                entry.State = EntityState.Deleted;

                _context.SaveChanges();
            }
        }

        public Avatar SaveUserAvatar(int userId, System.IO.Stream avatarStream, string contentType)
        {
            if ((avatarStream == null) || (avatarStream.Length == 0))
            {
                return GetAvatarByUserId(userId);
            }

            string base64Url = ConvertStreamToBase64Url(avatarStream, contentType);
            var avatar = GetAvatarByUserId(userId);

            if (avatar == null)
            {
                avatar = new Avatar { UserId = userId };

                _context.Avatars.Add(avatar);
            }

            avatar.Base64Url = base64Url;
            _context.SaveChanges();

            return avatar;
        }

        public string GetDefaultAvatarUrl()
        {
            return DefaultAvatarUrl;
        }

        public void DeleteNewUsers(int[] userIds)
        {

            var users = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();

            if (users.Any(u => u.Roles.Count != 0))
            {
                throw new Exception("Operation failed. Target users can not have roles");
            }

            foreach (var user in users)
            {
                _context.Entry(user).State = EntityState.Deleted;

                if (user.Contacts != null)
                {
                    _context.Entry(user.Contacts).State = EntityState.Deleted;
                }
            }

            _context.SaveChanges();
        }

        public List<User> GetNewUsers()
        {
            var result = _context.Users.Where(u => u.Roles.Count() == 0).ToList();

            return result;
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _context.Dispose();
        }

        protected virtual void AddToRole(User user, RoleType roleType)
        {
            _userManager.AddToRole(user.Id, roleType.ToString());
        }

        private Avatar GetAvatarByUserId(int userId)
        {
            return _context.Avatars.SingleOrDefault(a => a.UserId == userId);
        }

        private Avatar GetDefaultAvatar()
        {
            var avatar = new Avatar { Base64Url = GetDefaultAvatarUrl() };

            return avatar;
        }

        private string ConvertStreamToBase64Url(System.IO.Stream stream, string contentType)
        {
            byte[] data = null;

            using (var binaryReader = new System.IO.BinaryReader(stream))
            {
                stream.Position = 0;
                data = binaryReader.ReadBytes((int)stream.Length);
            }

            string base64Data = Convert.ToBase64String(data);
            string base64Url = $"data:{contentType};base64,{base64Data}";

            return base64Url;
        }

        private void CreateUserBlankByRole(User user, RoleType role, Specialization specialization)
        {
            switch (role)
            {
                case RoleType.Mentor:
                    if (!_context.Mentors.Any(m => m.Id == user.Id))
                    {
                        _context.Mentors.Add(Mentor.GetBlank(user, specialization));
                    }

                    break;
                case RoleType.ScrumMaster:
                    if (!_context.ScrumMasters.Any(sm => sm.Id == user.Id))
                    {
                        _context.ScrumMasters.Add(ScrumMaster.GetBlank(user));
                    }

                    break;
                case RoleType.Trainee:
                    if (!_context.Trainees.Any(t => t.Id == user.Id))
                    {
                        _context.Trainees.Add(Trainee.GetBlank(user, specialization));
                    }

                    break;
                case RoleType.HR:
                    return;
                default:
                    throw new Exception("Invalid RoleType");
            }
        }
    }
}
