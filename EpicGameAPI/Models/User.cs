using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class User :IdentityUser
    {


        [Required]
        string FirstName { get; set; }
        
        [Required]
        string LastName { get; set; }
    }
}
