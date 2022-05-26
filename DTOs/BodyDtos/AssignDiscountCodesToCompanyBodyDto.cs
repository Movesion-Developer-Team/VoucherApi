namespace DTOs.BodyDtos
{
    public class AssignDiscountCodesToCompanyBodyDto
    {
        public int? DiscountId { get; set; }
        public int? CompanyId { get; set; }
        public int NumberOfDiscounts { get; set; }

        public double Price { get; set; }
    }
}
