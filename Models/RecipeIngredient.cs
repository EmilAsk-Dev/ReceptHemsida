using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class RecipeIngredient
    {
        [Required]
        public Guid RecipeId { get; set; }

        [Required]
        public Guid IngredientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Quantity { get; set; }

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; }
    }
}
