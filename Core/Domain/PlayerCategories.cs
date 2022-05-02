namespace Core.Domain
{
    public class PlayerCategories : EntityBase
    {
        public int? PlayerId { get; set; }
        public Player? Player { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
