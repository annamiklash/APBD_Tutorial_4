using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using APBD_Tutorial_4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APBD_Tutorial_4.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IStudentsDb _studentsDb;
        
        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
            _studentsDb = new SqlServerDb();
            
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing authorization header");
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(":");

            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Incorrect Authorization Header Value");
            }

            var username = credentials[0];
            var password = credentials[1];

            bool isValid =_studentsDb.CredentialsValid(username, password);

            if (!isValid)
            {
                return AuthenticateResult.Fail("Incorrect Login and Password");
            }

            var claims = GenerateClaimsForStudent(username, password);
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            
            return AuthenticateResult.Success(authenticationTicket);
        }

        private IEnumerable<Claim> GenerateClaimsForStudent(string username, string password)
        {
            var roles =_studentsDb.GetStudentRole(username, password);
            List<Claim> claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, username));

            return claims;
        }
    }
}