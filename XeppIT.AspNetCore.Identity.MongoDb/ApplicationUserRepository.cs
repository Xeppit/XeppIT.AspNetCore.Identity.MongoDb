using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    public class ApplicationUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _applicationUserCollection;

        public ApplicationUserRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);

            var mongoDatabase = client.GetDatabase("Identity");

            _applicationUserCollection = mongoDatabase.GetCollection<ApplicationUser>("ApplicationUser");
        }

        public IMongoCollection<ApplicationUser> GetCollection()
        {
            return _applicationUserCollection;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            try
            {
                await _applicationUserCollection.InsertOneAsync(user);
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq("Id", user.Id);
            var result = await _applicationUserCollection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
        }


        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _applicationUserCollection.Find(x => x.Id.Equals(userId)).FirstOrDefaultAsync();
        }


        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _applicationUserCollection.Find(x => x.UserName.Equals(userName)).FirstOrDefaultAsync();
        }
    }
}
