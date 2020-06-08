using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    public class ApplicationUser
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string NormalizedUserName { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string Email { get; set; }

        public virtual string NormalizedEmail { get; set; }

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        public virtual bool EmailConfirmed { get; set; }
        
        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }
        [BsonIgnoreIfNull]
        public virtual List<string> Roles { get; set; } = new List<string>();

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        [BsonIgnoreIfNull]
        public List<ApplicationUserToken> Tokens { get; set; } = new List<ApplicationUserToken>();
        private ApplicationUserToken GetToken(string loginProvider, string name)
            => Tokens
                .FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        
        public virtual void SetToken(string loginProvider, string name, string value)
        {
            var existingToken = GetToken(loginProvider, name);

            if (existingToken != null)
            {
                existingToken.Value = value;
                return;
            }

            Tokens.Add(new ApplicationUserToken
            {
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            });
        }

        public virtual string GetTokenValue(string loginProvider, string name)
        {
            return GetToken(loginProvider, name)?.Value;
        }

        public virtual void RemoveToken(string loginProvider, string name)
        {
            var existingToken = GetToken(loginProvider, name);

            if (existingToken == null) return;
            Tokens.Remove(existingToken);
            return;
        }

        [BsonIgnoreIfNull]
        public virtual List<ApplicationUserClaim> Claims { get; set; } = new List<ApplicationUserClaim>();

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new ApplicationUserClaim(claim));
        }

        public virtual void RemoveClaim(Claim claim)
        {
            Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);
        }

        public virtual void ReplaceClaim(Claim existingClaim, Claim newClaim)
        {
            var claimExists = Claims
                .Any(c => c.Type == existingClaim.Type && c.Value == existingClaim.Value);
            if (!claimExists)
            {
                // note: nothing to update, ignore, no need to throw
                return;
            }
            RemoveClaim(existingClaim);
            AddClaim(newClaim);
        }

        [BsonIgnoreIfNull]
        public virtual List<ApplicationUserLogin> Logins { get; set; } = new List<ApplicationUserLogin>();

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(new ApplicationUserLogin(login));
        }

        public virtual void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
        }
    }

}
