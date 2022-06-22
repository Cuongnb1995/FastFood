using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class ItemNews
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int? Hot { get; set; }
        public string? Photo { get; set; }
    }
}
