using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Ingredients_dbModel
    {
        [Key]
        public int IngredientID { get; set; }

        [Required]
        [StringLength(100)]
        public string IngredientName { get; set; }

        public ICollection<Recipes_Ingredients_dbModel> RecipesIngredients { get; set; }

        public string? Icons{ get; set; }
    }
}
