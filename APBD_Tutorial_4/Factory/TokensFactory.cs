using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Tutorial_4.Factory
{
    public class TokenFactory
    {
        public static AccessToken GenerateAccessToken(LoginRequest request, IConfiguration configuration, IStudentsDb studentsDb)
        {
            var claims = GenerateClaimsForStudent(request.Username, request.Password, studentsDb);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expirationDateTime = DateTime.Now.AddMinutes(Constants.ACCESS_TOKEN_EXP);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: expirationDateTime,
                signingCredentials: signingCredentials);

           var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
           
            AccessToken actualAccessToken = new AccessToken(accessToken, expirationDateTime, DateTime.Now);
            var student = studentsDb.GetStudentByIndex(request.Username);
            student.AccessToken = actualAccessToken;

            return actualAccessToken;
        }

        public static RefreshToken GenerateRefreshToken(LoginRequest request, IStudentsDb studentsDb, AccessToken accessToken)
        {
         string token = Guid.NewGuid().ToString();
         DateTime issueDate = accessToken.IssueDateTime;
        
         RefreshToken actualRefreshToken = new RefreshToken(token, issueDate);
         var student = studentsDb.GetStudentByIndex(request.Username);
         student.RefreshToken = actualRefreshToken;

         return actualRefreshToken;
        }
        
        private static IEnumerable<Claim> GenerateClaimsForStudent(string username, string password, IStudentsDb studentsDb)
        {
            var roles =studentsDb.GetStudentRole(username, password);
            List<Claim> claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, username));

            return claims;
        }
    }
}