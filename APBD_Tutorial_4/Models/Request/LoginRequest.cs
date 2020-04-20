namespace APBD_Tutorial_4.Model
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRequest()
        {
            
        }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}