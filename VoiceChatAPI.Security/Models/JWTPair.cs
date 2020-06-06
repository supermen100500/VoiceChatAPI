namespace VoiceChatAPI.Security.Models
{
    public class JWTPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
    }
}
