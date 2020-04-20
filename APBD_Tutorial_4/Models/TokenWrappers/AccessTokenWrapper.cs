using System;

namespace APBD_Tutorial_4.Model
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime TokenExpirationDateTime { get; set; }
        public DateTime IssueDateTime { get; set; }
        
        public AccessToken()
        {
        }

        public AccessToken(string token, DateTime tokenExpirationDateTime, DateTime issueDateTime)
        {
            Token = token;
            TokenExpirationDateTime = tokenExpirationDateTime;
            IssueDateTime = issueDateTime;
        }
    }
}