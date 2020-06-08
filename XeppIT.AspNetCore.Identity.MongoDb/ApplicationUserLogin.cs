namespace Microsoft.AspNetCore.Identity.MongoDB
{
	public class ApplicationUserLogin
	{
		public ApplicationUserLogin(string loginProvider, string providerKey, string providerDisplayName)
		{
			LoginProvider = loginProvider;
			ProviderDisplayName = providerDisplayName;
			ProviderKey = providerKey;
		}

		public ApplicationUserLogin(UserLoginInfo login)
		{
			LoginProvider = login.LoginProvider;
			ProviderDisplayName = login.ProviderDisplayName;
			ProviderKey = login.ProviderKey;
		}

		public string LoginProvider { get; set; }
		public string ProviderDisplayName { get; set; }
		public string ProviderKey { get; set; }

		public UserLoginInfo ToUserLoginInfo()
		{
			return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
		}
	}
}