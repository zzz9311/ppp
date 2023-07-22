﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Index("Username", IsUnique = true, Name = "Username_Index")]
    public class AppUser
    {
        public AppUser()
        {
            Id = Guid.NewGuid();    
        }
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}