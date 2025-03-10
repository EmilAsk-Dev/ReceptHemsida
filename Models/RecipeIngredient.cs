using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class RecipeIngredient
    {
        [Required]
        public string RecipeId { get; set; }

        [Required]
        public string IngredientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Quantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Unit { get; set; }

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; }
    }
}
