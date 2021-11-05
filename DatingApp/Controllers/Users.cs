using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
      public class Users : BaseAPIController
    {
        public DataContext Context { get; }
        public Users(DataContext context)
        {
            this.Context=context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await Context.Users.ToListAsync();       
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await Context.Users.FindAsync(id);       
        }
    }

    
}