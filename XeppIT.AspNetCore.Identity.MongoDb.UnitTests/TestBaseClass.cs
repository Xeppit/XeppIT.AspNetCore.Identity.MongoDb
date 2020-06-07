using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NUnit.Framework;

namespace XeppIT.AspNetCore.Identity.MongoDb.UnitTests
{
    public class TestBaseClass
    {
        protected IMongoDatabase Database;
        protected IMongoCollection<ApplicationUser> ApplicationUserCollection;
        protected IMongoCollection<ApplicationRole> ApplicationRoleCollection;

        protected IServiceProvider ServiceProvider;
        private readonly string _testingConnectionString = $"mongodb://root:admin@192.168.0.13:27017";

        [SetUp]
        public void BeforeEachTest()
        {
            var url = new MongoUrl(_testingConnectionString);

            var client = new MongoClient(url);

            Database = client.GetDatabase("Identity");

            ApplicationUserCollection = Database.GetCollection<ApplicationUser>("ApplicationUser");
            ApplicationRoleCollection = Database.GetCollection<ApplicationRole>("ApplicationRole");

            Database.DropCollection("ApplicationUser");
            Database.DropCollection("ApplicationRole");

            ServiceProvider = CreateServiceProvider<ApplicationUser, ApplicationRole>();
        }

        protected UserManager<ApplicationUser> GetUserManager()
            => ServiceProvider.GetService<UserManager<ApplicationUser>>();

        protected RoleManager<ApplicationRole> GetRoleManager()
            => ServiceProvider.GetService<RoleManager<ApplicationRole>>();

        protected IServiceProvider CreateServiceProvider<TUser, TRole>(Action<IdentityOptions> optionsProvider = null)
            where TUser : ApplicationUser
            where TRole : ApplicationRole
        {
            var services = new ServiceCollection();

            optionsProvider = optionsProvider ?? (options => { });

            services
                .RegisterMongoStores<TUser, TRole>(_testingConnectionString);
            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }

}
