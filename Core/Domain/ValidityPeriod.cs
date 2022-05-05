namespace Core.Domain
{
    public class ValidityPeriod : EntityBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
