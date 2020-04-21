using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using APBD_Tutorial_4.Factory;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Tutorial_4.Handlers
{
    public class RefreshTokenHandler
    {
        public static AuthResponse RefreshToken(AuthRequest request, IStudentsDb studentsDb,
            IConfiguration configuration)
        {
            List<Error> errorList = new List<Error>();

            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            var accessTokenHandler = new JwtSecurityTokenHandler();
            var token = accessTokenHandler.ReadToken(accessToken.Token) as JwtSecurityToken;

            var tokenClaims = token?.Claims;
            var claimsList = tokenClaims?.ToList();

            if (token == null)
            {
                Error error = new Error("token", token.ToString(),
                    "Token is null");
                errorList.Add(error);
            }

            if (accessToken.TokenExpirationDateTime > DateTime.Now)
            {
                Error error = new Error("expireDate",
                    accessToken.TokenExpirationDateTime.ToString(CultureInfo.CurrentCulture),
                    "Token has not expired yet");
                errorList.Add(error);
            }

            var nameIdentifier = claimsList?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentifier == null)
            {
                Error error = new Error("nameIdentifier", nameIdentifier,
                    "NameIdentifier from access token is null");
                errorList.Add(error);
            }

            if (!nameIdentifier.Equals(request.Index))
            {
                Error error = new Error("nameIdentifier", nameIdentifier,
                    "NameIdentifier is not equal to Index " + request.Index + " in the request");
                errorList.Add(error);
            }

            var hashedPassword = studentsDb.GetHashedPassword(request.Index);
            if (hashedPassword == null)
            {
                Error error = new Error("password", hashedPassword,
                    "Couldnt get password for Index " + request.Index);
                errorList.Add(error);
            }
            

            var loginRequest = new LoginRequest(request.Index, hashedPassword);

           // var newAccessToken = TokensGenerator.GenerateInitialAccessToken(loginRequest, configuration, studentsDb);
           var newAccessToken = TokensGenerator.GenerateNewAccessToken(tokenClaims, configuration, studentsDb);
            var newRefreshToken = TokensGenerator.GenerateRefreshToken(loginRequest, studentsDb, newAccessToken);

            return new AuthResponse(newAccessToken, newRefreshToken, request.Index, errorList);
        }
    }
}