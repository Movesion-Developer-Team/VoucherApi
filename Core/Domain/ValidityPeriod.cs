namespace Core.Domain
{
    public class ValidityPeriod : EntityBase
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
