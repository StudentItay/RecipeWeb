using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FavoriteModel
    {
        [Key]
        public int FavoriteId { get; set; }
        public int RecipeId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime Date { get; set; }
        public Boolean FavoriteIcon { get; set; }
        public Boolean LikeIcon { get; set; }
    }
}
