using XeppIT.AspNetCore.Identity.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using NUnit.Framework;

namespace XeppIT.AspNetCore.Identity.MongoDb.UnitTests
{
    [TestFixture()]
    public class CustomUserStoreUnitTests : TestBaseClass
    {
        ApplicationUser _testApplicationUser;
        private UserManager<ApplicationUser> _testUserManager;

        protected ApplicationUser GenerateApplicationUser()
        {
            return new ApplicationUser()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                PasswordHash = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }

        [SetUp]
        public async Task Setup()
        {
            _testApplicationUser = new ApplicationUser()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                PasswordHash = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            _testUserManager = GetUserManager();
        }
        
        [Test]
        public async Task GetUserIdAsync_ReturnsId()
        {
            var result = await _testUserManager.GetUserIdAsync(_testApplicationUser);

            Assert.AreSame(result, _testApplicationUser.Id, "Wrong id returned");
        }

        [Test]
        public async Task GetUserNameAsync_ReturnsUserName()
        {
            var result = await _testUserManager.GetUserNameAsync(_testApplicationUser);

            Assert.AreSame(result, _testApplicationUser.UserName, "Wrong name returned");
        }

        [Test()]
        public async Task SetUserNameAsyncTest()
        {
            var testName = "TestName";

            await _testUserManager.SetUserNameAsync(_testApplicationUser, testName);

            Assert.AreSame(_testApplicationUser.UserName, testName, "Wrong name returned");
        }

        [Test()]
        public async Task CreateAsyncTest()
        {
            var result = await _testUserManager.CreateAsync(_testApplicationUser);

            Assert.IsTrue(result.Succeeded, "Insert Failed");
        }

        [Test()]
        public async Task UpdateAsyncTest()
        {
            var updatedTestApplicationUser = new ApplicationUser()
            {
                Id = _testApplicationUser.Id,
                DisplayName = "Test1",
                UserName = "Test1",
                Email = "Test1",
                PasswordHash = "Test1",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);
            var updateResult = await _testUserManager.UpdateAsync(updatedTestApplicationUser);


            Assert.IsTrue(insertResult.Succeeded, "Insert Failed");
            Assert.IsTrue(updateResult.Succeeded, "Update Failed");
        }

        [Test()]
        public async Task  DeleteAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);
            var deleteResult = await _testUserManager.DeleteAsync(_testApplicationUser);

            Assert.IsTrue(insertResult.Succeeded, "Insert Failed");
            Assert.IsTrue(deleteResult.Succeeded, "Delete Failed");
        }

        [Test()]
        public async Task  FindByIdAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);
            var findResult = await _testUserManager.FindByIdAsync(_testApplicationUser.Id);

            Assert.IsTrue(insertResult.Succeeded, "Insert Failed");
            Assert.IsTrue(findResult.UserName == _testApplicationUser.UserName, "Find by Id Failed");
        }

        [Test()]
        public async Task FindByIdAsyncTest_NotValidId()
        {
            var findResult = await _testUserManager.FindByIdAsync(ObjectId.GenerateNewId().ToString());

            Assert.IsTrue(findResult == null, "Insert Failed");
        }

        [Test()]
        public async Task  FindByNameAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);
            var findResult = await _testUserManager.FindByIdAsync(_testApplicationUser.Id);

            Assert.IsTrue(insertResult.Succeeded, "Insert Failed");
            Assert.IsTrue(findResult.UserName == _testApplicationUser.UserName, "Find by UserName Failed");
        }

        [Test()]
        public async Task SetEmailAsyncTest()
        {
            var insertResult = await _testUserManager.SetEmailAsync(_testApplicationUser, "bla@bla.com");

            Assert.IsTrue(_testApplicationUser.Email == "bla@bla.com", "Insert Failed");
        }

        [Test()]
        public async Task GetEmailAsyncTest()
        {
            var result = await _testUserManager.GetEmailAsync(_testApplicationUser);

            Assert.AreSame(result, _testApplicationUser.Email, "Wrong email returned");
        }

        [Test()]
        public async Task GetEmailConfirmedAsyncTest()
        {
            var result = await _testUserManager.IsEmailConfirmedAsync(_testApplicationUser);

            Assert.IsTrue(result, "IsEmailConfirmed failed return false");
        }

        [Test()]
        public async Task SetEmailConfirmedAsyncTest()
        {
            _testApplicationUser.EmailConfirmed = false;
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);

            var token = await _testUserManager.GenerateEmailConfirmationTokenAsync(_testApplicationUser);
            var confirmEmailResult = await _testUserManager.ConfirmEmailAsync(_testApplicationUser, token);

            Assert.IsTrue(insertResult.Succeeded, "Insert user failed");
            Assert.IsTrue(confirmEmailResult.Succeeded, "Email confirm failed");
        }

        [Test()]
        public async Task FindByEmailAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);

            var findByEmailResult = await _testUserManager.FindByEmailAsync(_testApplicationUser.Email);

            Assert.IsTrue(insertResult.Succeeded, "Insert user failed");
            Assert.IsTrue(findByEmailResult.Email == _testApplicationUser.Email, "Find by email failed");
        }

        [Test()]
        public async Task GetNormalizedEmailAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);

            var findByEmailResult = await _testUserManager.FindByEmailAsync(_testApplicationUser.Email);

            Assert.IsTrue(insertResult.Succeeded, "Insert user failed");
            Assert.IsTrue(findByEmailResult.NormalizedEmail == _testApplicationUser.Email.ToUpper(), "Get normalized email failed");
        }

        [Test()]
        public async Task SetNormalizedEmailAsyncTest()
        {
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser);

            var findByEmailResult = await _testUserManager.FindByEmailAsync(_testApplicationUser.Email);

            Assert.IsTrue(insertResult.Succeeded, "Insert user failed");
            Assert.IsTrue(findByEmailResult.NormalizedEmail == _testApplicationUser.Email.ToUpper(), "Set normalized email failed");
        }

        [Test()]
        public async Task SetPasswordHashAsyncTest()
        {
            _testApplicationUser.PasswordHash = "";
            var insertResult = await _testUserManager.CreateAsync(_testApplicationUser, "testPassword");
            var findByEmailResult = await _testUserManager.FindByEmailAsync(_testApplicationUser.Email);

            Assert.IsTrue(insertResult.Succeeded, "Insert failed");
            Assert.IsTrue(await _testUserManager.CheckPasswordAsync(findByEmailResult, "testPassword"), "Set password failed");
        }

        [Test()]
        public async Task HasPasswordAsyncTest()
        {
            var insertResult = await _testUserManager.HasPasswordAsync(_testApplicationUser);

            Assert.IsTrue(insertResult, "Has password failed");
        }

        [Test()]
        public async Task AddToRoleAsyncTest()
        {
            const string role = "God";
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role);

            Assert.IsTrue(_testApplicationUser.Roles.Contains(role.ToUpper()));
        }

        [Test()]
        public async Task RemoveFromRoleAsyncTest()
        {
            const string role = "God";
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role);
            Assert.IsTrue(_testApplicationUser.Roles.Contains(role.ToUpper()));

            await _testUserManager.RemoveFromRoleAsync(_testApplicationUser, role);
            Assert.IsTrue(!_testApplicationUser.Roles.Contains(role.ToUpper()));
        }

        [Test()]
        public async Task GetRolesAsyncTest()
        {
            const string role1 = "God1";
            const string role2 = "God2";
            const string role3 = "God3";
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role1);
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role2);
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role3);

            Assert.IsTrue(_testApplicationUser.Roles.Contains(role1.ToUpper()));
            Assert.IsTrue(_testApplicationUser.Roles.Contains(role2.ToUpper()));
            Assert.IsTrue(_testApplicationUser.Roles.Contains(role3.ToUpper()));

            var roles = await _testUserManager.GetRolesAsync(_testApplicationUser);

            Assert.IsTrue(roles.Count == 3);
            Assert.IsTrue(roles.Contains(role1.ToUpper()));
            Assert.IsTrue(roles.Contains(role2.ToUpper()));
            Assert.IsTrue(roles.Contains(role3.ToUpper()));
        }

        [Test()]
        public async Task IsInRoleAsyncTest()
        {
            const string role1 = "God1";
            const string role2 = "God2";
            const string role3 = "God3";
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role1);
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role2);
            await _testUserManager.AddToRoleAsync(_testApplicationUser, role3);

            var result = await _testUserManager.IsInRoleAsync(_testApplicationUser, role2);

            Assert.IsTrue(result);
        }

        [Test()]
        public async Task GetUsersInRoleAsyncTest()
        {
            const string role1 = "God1";
            const string role2 = "God2";
            const string role3 = "God3";

            var userToInsert1 = GenerateApplicationUser();
            var userToInsert2 = GenerateApplicationUser();
            var userToInsert3 = GenerateApplicationUser();

            var insertResult1 = await _testUserManager.CreateAsync(userToInsert1);
            var insertResult2 = await _testUserManager.CreateAsync(userToInsert2);
            var insertResult3 = await _testUserManager.CreateAsync(userToInsert3);

            await _testUserManager.AddToRoleAsync(userToInsert1, role1);
            await _testUserManager.AddToRoleAsync(userToInsert1, role2);
            await _testUserManager.AddToRoleAsync(userToInsert1, role3);
            await _testUserManager.AddToRoleAsync(userToInsert2, role1);
            await _testUserManager.AddToRoleAsync(userToInsert3, role1);
            await _testUserManager.AddToRoleAsync(userToInsert3, role2);

            var role1List = await _testUserManager.GetUsersInRoleAsync(role1);
            var role2List = await _testUserManager.GetUsersInRoleAsync(role2);
            var role3List = await _testUserManager.GetUsersInRoleAsync(role3);

            Assert.IsTrue(role1List.Count == 3);
            Assert.IsTrue(role2List.Count == 2);
            Assert.IsTrue(role3List.Count == 1);
        }

        [Test()]
        public async Task Set_Get_Remove_TokenAsyncTest()
        {
            var insertResult1 = await _testUserManager.CreateAsync(_testApplicationUser);

            var setResult = await _testUserManager.SetAuthenticationTokenAsync(_testApplicationUser, "Test1", "Test2", "Test3");
            Assert.IsTrue(setResult.Succeeded, "Set token failed");

            var getResult = await _testUserManager.GetAuthenticationTokenAsync(_testApplicationUser, "Test1", "Test2");
            Assert.IsTrue(getResult == "Test3", "Get token failed");

            var removeResult = await _testUserManager.RemoveAuthenticationTokenAsync(_testApplicationUser, "Test1", "Test2");
            Assert.IsTrue(removeResult.Succeeded, "Remove token failed");
        }

        [Test()]
        public async Task Add_Get_Find_Remove_LoginAsyncTest()
        {
            var userToInsert1 = GenerateApplicationUser();
            var userToInsert2 = GenerateApplicationUser();
            var userToInsert3 = GenerateApplicationUser();

            await _testUserManager.CreateAsync(userToInsert1);
            await _testUserManager.CreateAsync(userToInsert2);
            await _testUserManager.CreateAsync(userToInsert3);

            var addResult1 = await _testUserManager.AddLoginAsync(userToInsert1, new UserLoginInfo("Test1", "Test2", "Test3"));
            var addResult2 = await _testUserManager.AddLoginAsync(userToInsert2, new UserLoginInfo("Test4", "Test5", "Test6"));
            var addResult3 = await _testUserManager.AddLoginAsync(userToInsert3, new UserLoginInfo("Test7", "Test8", "Test9"));
            Assert.IsTrue(addResult1.Succeeded, "Add login1 failed");
            Assert.IsTrue(addResult2.Succeeded, "Add login2 failed");
            Assert.IsTrue(addResult3.Succeeded, "Add login3 failed");

            var getResult = await _testUserManager.GetLoginsAsync(userToInsert2);
            Assert.IsTrue(getResult.Count == 1, "Get login failed");

            var findResult = await _testUserManager.FindByLoginAsync("Test4", "Test5");
            Assert.IsTrue(findResult.UserName == userToInsert2.UserName, "Find login failed");

            var removeResult = await _testUserManager.RemoveLoginAsync(userToInsert2,"Test4", "Test5");
            Assert.IsTrue(removeResult.Succeeded, "Remove login failed");
        }

        [Test()]
        public async Task Set_Get_SecurityStampAsyncTest()
        {
            await _testUserManager.CreateAsync(_testApplicationUser);
            var oldStamp = _testApplicationUser.SecurityStamp;
            Assert.IsTrue(!string.IsNullOrEmpty(oldStamp), "get stamp failed");

            var getResult = await _testUserManager.UpdateSecurityStampAsync(_testApplicationUser);
            Assert.IsTrue(_testApplicationUser.SecurityStamp != oldStamp, "set stamp failed");
        }
        
        [Test()]
        public async Task Set_Get_TwoFactorEnabledAsyncTest()
        {
            var insertResult1 = await _testUserManager.CreateAsync(_testApplicationUser);
            var setResult = await _testUserManager.SetTwoFactorEnabledAsync(_testApplicationUser, true);
            Assert.IsTrue(setResult.Succeeded, "Set_TwoFactorEnabledAsyncTest failed");

            var getResult = await _testUserManager.GetTwoFactorEnabledAsync(_testApplicationUser);
            Assert.IsTrue(getResult, "Get_TwoFactorEnabledAsyncTest failed");
        }


        //[Test()]
        //public async Task Set_Get_LockoutEndDateAsyncTest()
        //{
        //    var setResult = await _testUserManager.SetLockoutEndDateAsync(_testApplicationUser, DateTimeOffset.Now);
        //    Assert.IsTrue(setResult.Succeeded, "Set_LockoutEndDateAsyncTest failed");

        //    var getResult = await _testUserManager.GetLockoutEndDateAsync(_testApplicationUser);
        //    Assert.IsTrue(getResult == DateTime.Now, "Get_LockoutEndDateAsyncTest failed");
        //}


        //[Test()]
        //public async Task Increment_Get_Reset_AccessFailedCountAsyncTest()
        //{
        //    Assert.Fail();
        //}

        [Test()]
        public async Task Get_Set_LockoutEnabledAsyncTest()
        {
            await _testUserManager.CreateAsync(_testApplicationUser);

            var setResult = await _testUserManager.SetLockoutEnabledAsync(_testApplicationUser, false);
            Assert.IsTrue(setResult.Succeeded, "Set_LockoutEnabledAsyncTest failed");

            var getResult = await _testUserManager.IsLockedOutAsync(_testApplicationUser);
            Assert.IsTrue(!getResult, "Get_LockoutEnabledAsyncTest failed");

            var setResult2 = await _testUserManager.SetLockoutEnabledAsync(_testApplicationUser, true);
            Assert.IsTrue(setResult2.Succeeded, "Set_LockoutEnabledAsyncTest2 failed");

            var getResult2 = await _testUserManager.IsLockedOutAsync(_testApplicationUser);
            Assert.IsTrue(!getResult2, "Get_LockoutEnabledAsyncTest2 failed");
        }

    }
}
