namespace Core.Entities
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; private set; }
        public string Username { get; set; }

        public AuthenticationResult(bool isAuthenticated, string username)
        {
            IsAuthenticated = isAuthenticated;
            Username = username;
        }
    }
}