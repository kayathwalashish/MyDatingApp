using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IToken
    {
        string CreateToken(AppUser appUser );
    }
}