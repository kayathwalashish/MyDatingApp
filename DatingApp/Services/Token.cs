using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IO;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.Services
{
    public class Token : IToken
    {
        private readonly SymmetricSecurityKey _key;
        private BinaryReader JwtRegisteredClaimNames;

        public Token(IConfiguration _config)
        {
            _key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));           
        }

        public string CreateToken(AppUser appUser)
        {
            //throw new NotImplementedException();

            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId,appUser.UserName)
            };

            var cred = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                    Subject = new ClaimsIdentity(claims),
                    Expires=DateTime.Now.AddDays(7),
                    SigningCredentials=cred
            };     

            var tokenhandler = new JwtSecurityTokenHandler();      

            var token = tokenhandler.CreateToken(tokenDescriptor);

            return tokenhandler.WriteToken(token);

        }
    }
}