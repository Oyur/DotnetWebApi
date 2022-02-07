using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetWebApi.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "Varchar(64)",Order = 2)]
        public string ProductName { get; set; }

        [Required]
        [Column(Order = 3)]
        public int Price { get; set; }

        [Required]
        [Column(Order = 4)]
        public int Quantity { get; set; }

        [Required]
        [Column(Order = 5)]
        public string Picture { get; set; }

        [Column(Order = 6)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(Order = 7)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;


        [ForeignKey("CategoryInfo")] //ตั้งชื่ออะไรก็ได้
        [Required]
        public int CategoryId { get; set; }

        public virtual Category CategoryInfo { get; set; }

        [NotMapped] // ไม่ต้องสร้างใน table Product
        public string CategoryName { get; set; }
    }
}