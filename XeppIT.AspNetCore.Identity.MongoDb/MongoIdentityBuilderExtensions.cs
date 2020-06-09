using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace XeppIT.AspNetCore.Identity.MongoDb
{
    public static class MongoIdentityBuilderExtensions
    {
        public static IdentityBuilder RegisterMongoStores<TUser, TRole>(this IServiceCollection services, string connectionString)
            where TUser : class
            where TRole : class
        {
            // Add identity types
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddDefaultTokenProviders();


            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("Identity");

            var applicationUserCollection = database.GetCollection<ApplicationUser>("ApplicationUsers");
            var applicationRoleCollection = database.GetCollection<ApplicationRole>("ApplicationRoles");

            services.AddSingleton(provider => applicationUserCollection);
            services.AddSingleton(provider => applicationRoleCollection);

            services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }
    }
}
