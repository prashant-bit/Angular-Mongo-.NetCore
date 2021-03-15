﻿using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
