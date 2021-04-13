using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Proteo5.HL
{
    public static class TokenHL
    {
        public static Result<dynamic> TokenValid(string signedAndEncodedToken, string plainTextSecurityKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(plainTextSecurityKey));
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey
                };

                SecurityToken validatedToken;
                var claims = tokenHandler.ValidateToken(signedAndEncodedToken,
                    tokenValidationParameters, out validatedToken);
                string email = claims.Claims.Where(item => item.Type.EndsWith("/emailaddress")).FirstOrDefault().Value;
                string name = claims.Claims.Where(item => item.Type.EndsWith("/name")).FirstOrDefault().Value;
                string surname = claims.Claims.Where(item => item.Type.EndsWith("/surname")).FirstOrDefault().Value;
                string user = claims.Claims.Where(item => item.Type.EndsWith("/nameidentifier")).FirstOrDefault().Value;

                var newTokenResult = GetToken(user, name, surname, email, plainTextSecurityKey);
                return newTokenResult;

            }
            catch (Exception ex)
            {
                return new Result<dynamic> { State = ResultsStates.unsuccess, Message = "The token is not valid" };
            }
        }

        public static Result<dynamic> GetToken(string user, string name, string surname, string email, string key)
        {
            var HMACKey = key;
            SecurityKey _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(HMACKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Surname, surname),
                    new Claim(ClaimTypes.NameIdentifier, user)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            dynamic dr = new ExpandoObject();
            dr.token = tokenHandler.WriteToken(token);
            dr.user = user;
            dr.name = name;
            dr.surname = surname;
            dr.email = email;
            return new Result<dynamic>() { State = ResultsStates.success, Data = dr };
        }
    }
}
