using System.Collections.Generic;

namespace APBD_Tutorial_4.Model
{
    public class AuthResponse
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Index { get; set; }
        public List<Error> Errors { get; set; }

        public AuthResponse(AccessToken accessToken, RefreshToken refreshToken, string index, List<Error> errors)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Index = index;
            Errors = errors;

        }

        public AuthResponse(List<Error> errors)
        {
            Errors = errors;
        }
    }
}