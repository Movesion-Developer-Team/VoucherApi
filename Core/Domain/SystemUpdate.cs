namespace Core.Domain
{
    public class SystemUpdate : EntityBase
    {
        public DateTimeOffset? UpdateDate { get; set; }
        public int? RefreshedCodesQuantity { get; set; }
        public int? ActiveReservations { get; set; }
    }
}
