using System;

namespace APBD_Tutorial_4.Model
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime IssueDateTime { get; set; }

        public RefreshToken()
        {
        }

        public RefreshToken(string token, DateTime issueDateTime)
        {
            Token = token;
            IssueDateTime = issueDateTime;
        }
    }
}