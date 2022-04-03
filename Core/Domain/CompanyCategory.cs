namespace Core.Domain
{
    public class CompanyCategory : EntityBase
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
    }
}
