using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
