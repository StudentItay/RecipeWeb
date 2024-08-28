using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class AddComment
    {

        [Key]
        public int CommentID { get; set; }
        public int RecipeID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 5)] // נוסיף את התכונה הזו כדי לוודא שהדירוג נמצא בטווח המתאים (לדוג', מ-1 עד 5)
        public int Rating { get; set; }

        [Required]
        [MaxLength(1000)]
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
