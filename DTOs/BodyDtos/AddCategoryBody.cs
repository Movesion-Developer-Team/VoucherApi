namespace DTOs.BodyDtos
{
    public class AddCategoryBody
    {
        public int? CompanyId { get; set; }
        public int? CategoryId { get; set; }
        public string? CompanyName { get; set; }
        public string? CategoryName { get; set; }
    }
}
