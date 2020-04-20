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
         public static AuthResponse RefreshToken(AuthRequest request, IStudentsDb studentsDb, IConfiguration configuration)
        {
            List<Error> list = new List<Error>();

            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

           //  var tokenHandler = new JwtSecurityTokenHandler();
           //  var token = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
           // // var tokenValidTo = token?.ValidTo.ToLocalTime();
           //
           //
           // long expiryDateUnix = long.Parse(token.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value);
           //  var expireDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
           //  var lifetimeInMinutes = tokenHandler.TokenLifetimeInMinutes;
           //  var startDate = expireDateUtc.Subtract(new TimeSpan(0, 0, lifetimeInMinutes , 0));
           
           var accessTokenHandler = new JwtSecurityTokenHandler();
           var token = accessTokenHandler.ReadToken(accessToken.Token) as JwtSecurityToken;
           
           var tokenClaims = token?.Claims;
           var claimsList = tokenClaims?.ToList();
          
           if (token == null)
           {
               Error error = new Error("token", token.ToString(), 
                   "Token is null" );
               list.Add(error);
               return new AuthResponse(list);
           }
           
            if (accessToken.TokenExpirationDateTime > DateTime.Now)
            {
                Error error = new Error("expireDate", accessToken.TokenExpirationDateTime.ToString(CultureInfo.CurrentCulture), 
                    "Token has not expired yet" );
                list.Add(error);
                return new AuthResponse(list);
            }

            var nameIdentifier = claimsList?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentifier == null)
            {
                Error error = new Error("nameIdentifier", nameIdentifier, 
                    "NameIdentifier from access token is null" );
                list.Add(error);
                return new AuthResponse(list);
            }

            if (!nameIdentifier.Equals(request.Index))
            {
                Error error = new Error("nameIdentifier", nameIdentifier, 
                    "NameIdentifier is not equal to Index " + request.Index + " in the request");
                list.Add(error);
                return new AuthResponse(list);
            }

            string password = studentsDb.GetPassword(request.Index);
            if (password == null)
            {
                Error error = new Error("password", password, 
                    "Couldnt get password for Index " + request.Index);
                list.Add(error);
                return new AuthResponse(list);
            }

            var loginRequest = new LoginRequest(request.Index, password);

            var newAccessToken = TokenFactory.GenerateAccessToken(loginRequest,configuration,studentsDb);
            var newRefreshToken = TokenFactory.GenerateRefreshToken(loginRequest, studentsDb, newAccessToken);
            
            return new AuthResponse(newAccessToken, newRefreshToken,request.Index, list);
        }
    }
}