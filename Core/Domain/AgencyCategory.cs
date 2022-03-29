namespace Core.Domain
{
    public class AgencyCategory : EntityBase
    {
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
    }
}
