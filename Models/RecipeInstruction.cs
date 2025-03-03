using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ReceptHemsida.Models
{
    public class RecipeInstruction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StepNumber { get; set; } // För att kunna sortera stegen i rätt ordning

        [Required]
        public string InstructionText { get; set; } // Själva instruktionen

        [Required]
        public Guid RecipeId { get; set; } // Kopplar instruktionen till ett recept

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
    }
}
