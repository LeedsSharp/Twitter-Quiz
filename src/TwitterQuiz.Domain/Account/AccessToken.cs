namespace TwitterQuiz.Domain.Account
{
    public class AccessToken
    {
        public string ProviderType { get; set; }
        public string PublicAccessToken { get; set; }
        public string TokenSecret { get; set; }
    }
}
