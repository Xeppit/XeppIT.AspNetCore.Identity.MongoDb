#pragma warning disable CS1998

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    public class CustomUserStore :
        IQueryableUserStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserAuthenticationTokenStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>,
        IUserSecurityStampStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser>
        
	{
        private readonly IMongoCollection<ApplicationUser> _applicationUserCollection;

        public CustomUserStore(IMongoCollection<ApplicationUser> applicationUserCollection)
        {
            _applicationUserCollection = applicationUserCollection;
        }

        public IQueryable<ApplicationUser> Users => _applicationUserCollection.AsQueryable();


        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }


        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }


        public async Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)

        {
            user.UserName = userName;
        }


        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.NormalizedUserName;
        }


        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)

        {
            user.NormalizedUserName = normalizedName;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var insertOptions = new InsertOneOptions() {BypassDocumentValidation = false};

                await _applicationUserCollection.InsertOneAsync(user, insertOptions, cancellationToken);
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var replaceOptions = new ReplaceOptions() { BypassDocumentValidation = false, IsUpsert = false};

            var result =
                await _applicationUserCollection.ReplaceOneAsync(filter: 
                    x => x.Id == user.Id, 
                    user, 
                    replaceOptions, 
                    cancellationToken);

            if (!result.IsModifiedCountAvailable || !result.IsAcknowledged) return IdentityResult.Failed();

            return result.ModifiedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {


            var result = await _applicationUserCollection.DeleteOneAsync(
                u => u.Id == user.Id, new DeleteOptions(), cancellationToken);

            if (!result.IsAcknowledged) return IdentityResult.Failed();

            return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _applicationUserCollection.Find(
                x => x.Id.Equals(userId)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _applicationUserCollection.Find(
                x => x.NormalizedUserName.Equals(normalizedUserName)).FirstOrDefaultAsync(cancellationToken);
        }


        public async Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)

        {
            user.Email = email;
        }


        public async Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.Email;
        }


        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.EmailConfirmed;
        }


        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)

        {
            user.EmailConfirmed = confirmed;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _applicationUserCollection.Find(
                x => x.NormalizedEmail.Equals(normalizedEmail)).FirstOrDefaultAsync(cancellationToken);
        }


        public async Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.NormalizedEmail;
        }


        public async Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)

        {
            user.NormalizedEmail = normalizedEmail;
        }


        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)

        {
            user.PasswordHash = passwordHash;
        }


        public async Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.PasswordHash;
        }


        public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }


        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)

        {
            user.AddRole(roleName);
        }


        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)

        {
            user.RemoveRole(roleName);
        }


        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.Roles;
        }


        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)

        {
            return user.Roles.Contains(roleName);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _applicationUserCollection.Find(
                x => x.Roles.Contains(roleName)).ToListAsync(cancellationToken);
        }


        public async Task SetTokenAsync(ApplicationUser user, string loginProvider, string name, string value,

            CancellationToken cancellationToken)
        {
            user.SetToken(loginProvider, name, value);
        }


        public async Task RemoveTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)

        {
            user.RemoveToken(loginProvider, name);
        }


        public async Task<string> GetTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)

        {
            return user.GetTokenValue(loginProvider, name);
        }

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.AddLogin(login);
        }

        public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey,
            CancellationToken cancellationToken)
        {
            user.RemoveLogin(loginProvider, providerKey);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.Logins
                .Select(l => l.ToUserLoginInfo())
                .ToList();
        }

        public async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return await _applicationUserCollection
                .Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey))
                .FirstOrDefaultAsync(cancellationToken);
        }


        public async Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)

        {
            user.SecurityStamp = stamp ;
        }


        public async Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)

        {
            return user.SecurityStamp;
        }

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.TwoFactorEnabled;
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.LockoutEndDateUtc;
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEndDateUtc = lockoutEnd?.UtcDateTime;
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.AccessFailedCount++;
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
        }

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.LockoutEnabled;
        }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
        }

        public void Dispose()
        {

        }
    }
}
