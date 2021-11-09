using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext Context;
        public IToken Token { get; }
        public AccountController(DataContext context,IToken token)
        {
            this.Token = token;
            this.Context = context;
        }

       [HttpPost("register")]
       public async Task<ActionResult<User>> Register(Register register)
       {
           if (await CheckUserAlreadyExist(register.UserName)) return BadRequest("User Name Already Exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName=register.UserName.ToLower(),
                PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt=hmac.Key
            };   

            Context.Users.Add(user);
            await Context.SaveChangesAsync(); 
            return  new User{
                UserName= user.UserName,
                Token=Token.CreateToken(user)
            };   
       }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login login)
        {
            var user = await Context.Users.SingleOrDefaultAsync(x =>x.UserName==login.UserName);
            if (user==null) return BadRequest("Invalid UserName");

            var hmac = new HMACSHA512(user.PasswordSalt);
            var hmacHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(login.Password));

            for(int i=0;i<hmacHash.Length;i++)
            {
                    if (hmacHash[i] != user.PasswordHash[i])
                    {
                        return BadRequest("Invalid Password");
                    }
            }

            return new User{
                UserName=user.UserName,
                Token=Token.CreateToken(user)
            };
            
        }
        private async Task<bool> CheckUserAlreadyExist(string username)
        {
            return await Context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }

    }
}