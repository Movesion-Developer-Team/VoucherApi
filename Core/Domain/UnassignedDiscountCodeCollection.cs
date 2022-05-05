namespace Core.Domain
{
    public class UnassignedDiscountCodeCollection : EntityBase
    {
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
    }
}
