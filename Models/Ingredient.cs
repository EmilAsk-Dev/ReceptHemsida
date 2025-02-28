using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Models
{
    public class Ingredient
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
