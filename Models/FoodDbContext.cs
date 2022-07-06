using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class ItemUsers
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Ten khong duoc de trong")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email khong duoc de trong")]
        [RegularExpression(@"^\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*$", ErrorMessage = "Email sai dinh dang")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Password khong duoc de trong")]
        public string Password { get; set; }
    }

    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Vui long nhap ten")]
        public string Name { get; set; }
    }
    public class FoodDbContext : DbContext
    {
        public DbSet<ItemUsers> Users { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<ItemProducts> Products { get; set; }
        public DbSet<ItemProductsTags>? ProductsTags { get; set; }
        public DbSet<ItemTags> Tags { get; set; }
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemRating> Ratings { get; set; }
        public DbSet<ItemCustomers> Customers { get; set; }
        public DbSet<ItemOrders> Orders { get; set; }
        public DbSet<ItemOrdersDetail> OrdersDetails { get; set; }
        public DbSet<ItemAdv> Advs { get; set; }

        private const string sqlConnectString = "Server=DESKTOP-OEACG0K; Database = FastFood1; UID=sa; password=1;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(sqlConnectString);
        }
    }
}
