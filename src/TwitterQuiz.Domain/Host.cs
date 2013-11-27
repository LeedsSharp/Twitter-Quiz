using System.Collections.Generic;
using SimpleAuthentication.Core;
using AccessToken = TwitterQuiz.Domain.Account.AccessToken;

namespace TwitterQuiz.Domain
{
    public class Host
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string Picture { get; set; }
        public IList<AccessToken> AccessTokens { get; set; }

        public Host()
        {
            AccessTokens = new List<AccessToken>();
        }

        public static Host FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            var newUser = new Host
            {
                Username = authenticatedClient.UserInformation.UserName,
                Name = authenticatedClient.UserInformation.Name,
                Email = authenticatedClient.UserInformation.Email,
                Locale = authenticatedClient.UserInformation.Locale,
                Picture = authenticatedClient.UserInformation.Picture
            };
            var accessToken = new AccessToken
            {
                PublicAccessToken = authenticatedClient.AccessToken.PublicToken,
                ProviderType = authenticatedClient.ProviderName
            };
            newUser.AccessTokens.Add(accessToken);

            return newUser;
        }
    }
}
