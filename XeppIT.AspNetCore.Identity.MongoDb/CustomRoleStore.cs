using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    public class CustomRoleStore : IRoleStore<ApplicationRole>
    {
        private readonly IMongoCollection<ApplicationRole> _applicationUserCollection;

        public CustomRoleStore(IMongoCollection<ApplicationRole> applicationUserCollection)
        {
            _applicationUserCollection = applicationUserCollection;
        }

		public virtual async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken token)
		{
			await _applicationUserCollection.InsertOneAsync(role, cancellationToken: token);
			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken token)
		{
			var result = await _applicationUserCollection.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: token);
			// todo low priority result based on replace result
			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken token)
		{
			var result = await _applicationUserCollection.DeleteOneAsync(r => r.Id == role.Id, token);
			// todo low priority result based on delete result
			return IdentityResult.Success;
		}

		public virtual async Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
			=> role.Id;

		public virtual async Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
			=> role.Name;

		public virtual async Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
			=> role.Name = roleName;

		// note: can't test as of yet through integration testing because the Identity framework doesn't use this method internally anywhere
		public virtual async Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
			=> role.NormalizedName;

		public virtual async Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
			=> role.NormalizedName = normalizedName;

		public virtual Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken token)
			=> _applicationUserCollection.Find(r => r.Id == roleId)
				.FirstOrDefaultAsync(token);

		public virtual Task<ApplicationRole> FindByNameAsync(string normalizedName, CancellationToken token)
			=> _applicationUserCollection.Find(r => r.NormalizedName == normalizedName)
				.FirstOrDefaultAsync(token);

		public virtual IQueryable<ApplicationRole> Roles
			=> _applicationUserCollection.AsQueryable();

		public void Dispose()
        {
        }
    }
}
