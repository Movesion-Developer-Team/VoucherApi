using System.ComponentModel;

namespace Core.Domain
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Player>? Players { get; set; }
        public ICollection<Voucher>? Vouchers { get; set; }
        public Image? Image { get; set; }
        public List<PlayerCategories>? PlayerCategories { get; set; }
    }
}
