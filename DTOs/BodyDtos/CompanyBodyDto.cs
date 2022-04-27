namespace DTOs.BodyDtos
{
    public class CompanyBodyDto : BaseBody
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? NumberOfEmployees { get; set; }
    }
}
