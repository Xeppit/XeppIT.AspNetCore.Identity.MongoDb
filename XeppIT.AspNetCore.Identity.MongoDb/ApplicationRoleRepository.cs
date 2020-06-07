using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    class ApplicationRoleRepository
    {
        private readonly IMongoCollection<ApplicationRole> _applicationRoleCollection;

        public ApplicationRoleRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);

            var mongoDatabase = client.GetDatabase("Identity");

            _applicationRoleCollection = mongoDatabase.GetCollection<ApplicationRole>("ApplicationRole");
        }

        public IMongoCollection<ApplicationRole> GetCollection()
        {
            return _applicationRoleCollection;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            try
            {
                await _applicationRoleCollection.InsertOneAsync(role);
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert role {role.Name}." });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            var filter = Builders<ApplicationRole>.Filter.Eq("Id", role.Id);
            var result = await _applicationRoleCollection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.Name}." });
        }


        public async Task<ApplicationRole> FindByIdAsync(string roleId)
        {
            return await _applicationRoleCollection.Find(x => x.Id.Equals(roleId)).FirstOrDefaultAsync();
        }


        public async Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            return await _applicationRoleCollection.Find(x => x.Name.Equals(roleName)).FirstOrDefaultAsync();
        }
    }
}
