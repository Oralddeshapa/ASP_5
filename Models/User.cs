using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TeaChair.Models
{
    public class User : IdentityUser
    {
        public int Points { get; set; }
    }
}
