using ReceptHemsida.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class Follower
    {
        [Required]
        public string FollowerId { get; set; }

        [Required]
        public string FollowingId { get; set; }

        [ForeignKey("FollowerId")]
        public virtual ApplicationUser FollowerUser { get; set; }

        [ForeignKey("FollowingId")]
        public virtual ApplicationUser FollowingUser { get; set; }

        //Hej
    }
}
