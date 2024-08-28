using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Recipes_Ingredients_dbModel
    {

        [Key]
        public int RecipeIngredientID { get; set; }

        [Required]
        [StringLength(100)]
        public string RecipeName { get; set; }

        [Required]
        public int RecipeID { get; set; }

        [Required]
        [StringLength(100)]
        public string IngredientName { get; set; }

        [Required]
        public int IngredientID { get; set; }

        [StringLength(100)]
        public string? Quantity { get; set; }

        [ForeignKey("RecipeID")]
        public Recipes_dbModel Recipe { get; set; }

        [ForeignKey("IngredientID")]
        public Ingredients_dbModel Ingredient { get; set; }
    }
}
