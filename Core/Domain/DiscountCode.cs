namespace Core.Domain
{
    public class DiscountCode : EntityBase
    {
        public string? Code { get; set; }
        public Discount? Discount { get; set; }
        public int? UnassignedCollectionId { get; set; }
        public UnassignedDiscountCodeCollection? UnassignedDiscountCodeCollections { get; set; }
        
    }
}
