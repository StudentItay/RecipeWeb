using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class RecipeWebDbContext : DbContext
    {
        public RecipeWebDbContext(DbContextOptions<RecipeWebDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ingredients_dbModel> Ingredients { get; set; }
        public DbSet<Recipes_dbModel> Recipes { get; set; }
        public DbSet<Recipes_Ingredients_dbModel> RecipesIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // קונפיגורציה עבור Ingredients_dbModel
            builder.Entity<Ingredients_dbModel>().ToTable("Ingredients_db");
            builder.Entity<Ingredients_dbModel>().HasKey(i => i.IngredientID);
            builder.Entity<Ingredients_dbModel>().Property(i => i.IngredientName).IsRequired().HasMaxLength(100);
            builder.Entity<Ingredients_dbModel>().Property(i => i.Icons);

            // קונפיגורציה עבור Recipes_dbModel
            builder.Entity<Recipes_dbModel>().ToTable("Recipes_db");
            builder.Entity<Recipes_dbModel>().HasKey(r => r.RecipeID);
            builder.Entity<Recipes_dbModel>().Property(r => r.RecipeName).IsRequired().HasMaxLength(100);
            builder.Entity<Recipes_dbModel>().Property(r => r.Description);
            builder.Entity<Recipes_dbModel>().Property(r => r.Category).HasMaxLength(50);
            builder.Entity<Recipes_dbModel>().Property(r => r.Info);
            builder.Entity<Recipes_dbModel>().Property(r => r.PreparationTime).HasMaxLength(20);
            builder.Entity<Recipes_dbModel>().Property(r => r.DifficultyLevel).HasMaxLength(20);
            builder.Entity<Recipes_dbModel>().Property(r => r.Calories).HasMaxLength(50);
            builder.Entity<Recipes_dbModel>().Property(r => r.Entrances);
            builder.Entity<Recipes_dbModel>().Property(r => r.ImageRecipe);
            builder.Entity<Recipes_dbModel>().Property(r => r.CreationDate);

            // קונפיגורציה עבור Recipes_Ingredients_dbModel
            builder.Entity<Recipes_Ingredients_dbModel>().ToTable("Recipes_Ingredients_db");
            builder.Entity<Recipes_Ingredients_dbModel>().HasKey(ri => ri.RecipeIngredientID);
            builder.Entity<Recipes_Ingredients_dbModel>().Property(ri => ri.RecipeName).IsRequired().HasMaxLength(100);
            builder.Entity<Recipes_Ingredients_dbModel>().Property(ri => ri.IngredientName).IsRequired().HasMaxLength(100);
            builder.Entity<Recipes_Ingredients_dbModel>().Property(ri => ri.Quantity).HasMaxLength(100);
            builder.Entity<Recipes_Ingredients_dbModel>().HasOne(ri => ri.Recipe).WithMany(r => r.RecipesIngredients).HasForeignKey(ri => ri.RecipeID);
            builder.Entity<Recipes_Ingredients_dbModel>().HasOne(ri => ri.Ingredient).WithMany(i => i.RecipesIngredients).HasForeignKey(ri => ri.IngredientID);

            // קונפיגורציה נוספת במידת הצורך
        }
    }
}
