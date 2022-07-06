using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    [Table("Adv")]
    public class ItemAdv
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string? Href { get; set; }
        public int Position { get; set; }
    }
}
