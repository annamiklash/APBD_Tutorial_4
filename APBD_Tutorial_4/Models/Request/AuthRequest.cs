namespace APBD_Tutorial_4.Model
{
    public class AuthRequest
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Index { get; set; }

        public AuthRequest(AccessToken accessToken, RefreshToken refreshToken, string index)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Index = index;
        }

        public AuthRequest()
        {
        }
    }
    
    
}