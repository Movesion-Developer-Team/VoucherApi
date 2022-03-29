namespace Core.Domain
{
    public class PlayerContact : EntityBase
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public int PhoneNumber { get; set; }
        public Player? Player { get; set; }
        public int PlayerId { get; set; }   

    }
}
