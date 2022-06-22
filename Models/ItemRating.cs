using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    [Table("Rating")]
    public class ItemRating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Star { get; set; }
    }
}
