using System.Collections.Generic;
using SimpleAuthentication.Core;

namespace TwitterQuiz.Domain.Account
{
    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Locale { get; set; }
        public string Picture { get; set; }
        public IList<AccessToken> AccessTokens { get; set; }
        public bool IsAuthenticated { get; set; }

        public User()
        {
            AccessTokens = new List<AccessToken>();
        }

        public static User FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            var newUser = new User
                {
                    Username = authenticatedClient.UserInformation.UserName,
                    Name = authenticatedClient.UserInformation.Name,
                    Email = authenticatedClient.UserInformation.Email,
                    Gender = GenderTypeHelpers.ToGenderType(authenticatedClient.UserInformation.Gender.ToString()),
                    Locale = authenticatedClient.UserInformation.Locale,
                    Picture = authenticatedClient.UserInformation.Picture
                };
            AccessToken accessToken = new AccessToken
                {
                    PublicAccessToken = authenticatedClient.AccessToken.PublicToken,
                    ProviderType = authenticatedClient.ProviderName
                };
            newUser.AccessTokens.Add(accessToken);

            return newUser;
        }
    }
}
