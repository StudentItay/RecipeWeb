using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Recipes_dbModel
    {
        [Key]
        public int RecipeID { get; set; }


        [Required]
        [StringLength(100)]
        public string RecipeName { get; set; }

        public string? Description { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        public string? Info { get; set; }

        [StringLength(20)]
        public string? PreparationTime { get; set; }

        [StringLength(20)]
        public string? DifficultyLevel { get; set; }

        [StringLength(50)]
        public string? Calories { get; set; }

        public int? Entrances { get; set; }

        public string? ImageRecipe { get; set; }

        public DateTime? CreationDate { get; set; }

        public ICollection<Recipes_Ingredients_dbModel> RecipesIngredients { get; set; }

        

    }
}
