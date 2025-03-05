using ReceptHemsida.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class Recipe
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public RecipeCategory Category { get; set; }

        public int CookTime { get; set; }

        public string Difficulty { get; set; }
        // Relation till instruktionerna
        public virtual ICollection<RecipeInstruction> Instructions { get; set; } = new List<RecipeInstruction>();
        public List<string> Tags { get; set; }= new List<string>();
        public string ImageUrl => $"/images/{Title.Replace(" ", "").ToLower()}.jpg";

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
