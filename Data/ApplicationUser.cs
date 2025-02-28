using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ReceptHemsida.Models;

namespace ReceptHemsida.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Follower> Followers { get; set; }
        public virtual ICollection<Follower> Following { get; set; }
    }
}