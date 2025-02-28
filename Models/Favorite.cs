using ReceptHemsida.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class Favorite
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
    }
}
