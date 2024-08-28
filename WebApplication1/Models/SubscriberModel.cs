using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SubscriberModel
    {
        [Key]
        public int SubscriberID { get; set; }
        public string Email { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
