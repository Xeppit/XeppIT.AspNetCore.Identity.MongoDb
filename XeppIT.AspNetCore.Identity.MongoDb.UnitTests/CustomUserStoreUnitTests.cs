using XeppIT.AspNetCore.Identity.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace XeppIT.AspNetCore.Identity.MongoDb.UnitTests
{
    [TestFixture()]
    public class CustomUserStoreUnitTests : TestBaseClass
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task GetUserIdAsync_ReturnsId()
        {
            var manager = GetUserManager();

            var id = Guid.NewGuid().ToString();

            var applicationUser = new ApplicationUser()
            {
                Id = id
            };

            var result = await manager.GetUserIdAsync(applicationUser);

            Assert.AreSame(result, id, "Wrong id returned");
        }

        [Test]
        public async Task GetUserNameAsync_ReturnsUserName()
        {
            var manager = GetUserManager();

            var userName = Guid.NewGuid().ToString();

            var applicationUser = new ApplicationUser()
            {
                UserName = userName
            };

            var result = await manager.GetUserIdAsync(applicationUser);

            Assert.AreSame(result, userName, "Wrong name returned");
        }

        [Test()]
        public void SetUserNameAsyncTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetNormalizedUserNameAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetNormalizedUserNameAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CreateAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void UpdateAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FindByIdAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FindByNameAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetEmailAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetEmailAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetEmailConfirmedAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetEmailConfirmedAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FindByEmailAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetNormalizedEmailAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetNormalizedEmailAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetPasswordHashAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetPasswordHashAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void HasPasswordAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void AddToRoleAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void RemoveFromRoleAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetRolesAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void IsInRoleAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetUsersInRoleAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetTokenAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void RemoveTokenAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetTokenAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void AddLoginAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void RemoveLoginAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetLoginsAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FindByLoginAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetSecurityStampAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetSecurityStampAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetTwoFactorEnabledAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetTwoFactorEnabledAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetLockoutEndDateAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetLockoutEndDateAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void IncrementAccessFailedCountAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void ResetAccessFailedCountAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetAccessFailedCountAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetLockoutEnabledAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SetLockoutEnabledAsyncTest()
        {
            Assert.Fail();
        }

    }
}
