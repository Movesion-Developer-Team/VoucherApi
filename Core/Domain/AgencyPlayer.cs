namespace Core.Domain
{
    public class AgencyPlayer : EntityBase
    {
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }

    }
}
