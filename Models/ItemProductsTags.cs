using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class ItemProductsTags
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TagId { get; set; }
    }
}
